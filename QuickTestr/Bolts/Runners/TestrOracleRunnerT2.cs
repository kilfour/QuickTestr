using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;

namespace QuickTestr.Bolts.Runners;

/// <summary>
/// Runs an oracle-based Testr by comparing expected and actual behavior.
/// Use for Testrs defined through Expected and Actual.
/// </summary>
public class TestrOracleRunnerT2<TInput1, TInput2, TResult>(
    FuzzrOf<TInput1> fuzzrOfT1,
    FuzzrOf<TInput2> fuzzrOfT2,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<TInput1, TInput2, TResult> expected,
    Func<TInput1, TInput2, TResult> actual,
    Func<TInput1, TInput2, int>? deliberation,
    int? deliberationTarget,
    string testName,
    string fileName,
    bool UseBuiltInReducers) : TestrRunner<TInput1>
{
    /// <summary>
    /// Gets the display name of this Testr.
    /// Use when you need the configured name for reporting or storage.
    /// </summary>
    public override string TestName { get; } = testName;

    protected override CheckrOf<Case> GetCheckr() =>
        from showr in Showr.ForInput()
        from format in Combine.Checkrs(formatters)
        from input in Checkr.Input("Input", Fuzzr.Tuple(fuzzrOfT1, fuzzrOfT2), shrinkers)
        from expectedResult in Checkr.ActCarefully("Expected", () => expected(input.Item1, input.Item2))
        from actualResult in Checkr.ActCarefully("Actual", () => actual(input.Item1, input.Item2))
        from traceExpectedValue in Checkr.TraceWhen("Expected", () => !expectedResult.Threw, () => expectedResult.Value)
        from traceExpectedException in Checkr.TraceWhen("Expected", () => expectedResult.Threw, () => GetExceptionReport(expectedResult.Exception!))
        from traceActualValue in Checkr.TraceWhen("Actual  ", () => !actualResult.Threw, () => actualResult.Value)
        from traceActualException in Checkr.TraceWhen("Actual  ", () => actualResult.Threw, () => GetExceptionReport(actualResult.Exception!))
        from expectation in Checkr.Expect(TestName, () => CheckResults(expectedResult, actualResult))
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
            DeliberationPolicy = deliberation == null ? null :
                a => a.InputsNamed<(TInput1, TInput2)>("Input", a => deliberation(a.Item1, a.Item2)),
            DeliberationTarget = deliberationTarget == null ? null : deliberationTarget,
            ShrinkMode = UseBuiltInReducers ? a.ShrinkMode | ShrinkMode.Reduction : a.ShrinkMode,
            ReportMode = a.ReportMode & ~ReportMode.Labels & ~ReportMode.StackTrace | ReportMode.FinalTrace
        };
    }
}