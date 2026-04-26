using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;

namespace QuickTestr.Bolts;

/// <summary>
/// Runs an oracle-based Testr by comparing expected and actual behavior.
/// Use for Testrs defined through Expected and Actual.
/// </summary>
public class TestrOracleRunner<TInput, TResult>(
    FuzzrOf<TInput> fuzzr,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<TInput, TResult> Expected,
    Func<TInput, TResult> Actual,
    Func<TInput, int>? Deliberation,
    int? DeliberationTarget,
    string testName,
    string fileName,
    bool UseBuiltInReducers) : BaseTestrRunner<TInput>
{
    /// <summary>
    /// Gets the display name of this Testr.
    /// Use when you need the configured name for reporting or storage.
    /// </summary>
    public override string TestName { get; } = testName;

    protected override CheckrOf<Case> GetCheckr() =>
        from showr in Showr.ForInput()
        from format in Combine.Checkrs(formatters)
        from input in Checkr.Input("Input", fuzzr, shrinkers)
        from expected in Checkr.ActCarefully("Expected", () => Expected(input))
        from actual in Checkr.ActCarefully("Actual", () => Actual(input))
        from traceExpectedValue in Checkr.TraceWhen("Expected", () => !expected.Threw, () => expected.Value)
        from traceExpectedException in Checkr.TraceWhen("Expected", () => expected.Threw, () => GetExceptionReport(expected.Exception!))
        from traceActualValue in Checkr.TraceWhen("Actual  ", () => !actual.Threw, () => actual.Value)
        from traceActualException in Checkr.TraceWhen("Actual  ", () => actual.Threw, () => GetExceptionReport(actual.Exception!))
        from expectation in Checkr.Expect(TestName, () => CheckResults(expected, actual))
        select Case.Closed;

    private static string GetExceptionReport(Exception exception)
    {
        return $"{exception!.GetType().Name}: {exception.Message}";
    }

    private static bool CheckResults(DelayedResult<TResult> expected, DelayedResult<TResult> actual)
    {
        if (!expected.Threw && !actual.Threw)
        {
            if (expected.HasValue != actual.HasValue)
                return false;
            if (!expected.HasValue)
                return true;
            return Equals(expected.Value, actual.Value);
        }
        if (expected.Threw && actual.Threw)
            return EquivalentExceptions(expected.Exception, actual.Exception);
        return false;
    }

    private static bool EquivalentExceptions(Exception? expected, Exception? actual)
    {
        if (expected is null || actual is null)
            return expected is null && actual is null;
        return expected.GetType() == actual.GetType()
            && expected.Message == actual.Message
            && EquivalentExceptions(expected.InnerException, actual.InnerException);
    }

    protected override Func<CheckrConfig, CheckrConfig> GetConfig()
    {
        return a => a with
        {
            FileAs = fileName,
            StyleGuide = TheTestr.OracleStyleGuide,
            DeliberationPolicy = Deliberation == null ? null :
                a => a.InputsNamed<TInput>("Input", a => Deliberation(a)),
            DeliberationTarget = DeliberationTarget == null ? null : DeliberationTarget,
            ShrinkMode = UseBuiltInReducers ? a.ShrinkMode | ShrinkMode.Reduction : a.ShrinkMode,
            ReportMode = a.ReportMode & ~ReportMode.Warning & ~ReportMode.Labels & ~ReportMode.StackTrace | ReportMode.FinalTrace
        };
    }
}
