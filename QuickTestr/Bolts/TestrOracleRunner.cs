using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;

namespace QuickTestr.Bolts;

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
        if (expected.HasValue && actual.HasValue)
            return Equals(expected.Value, actual.Value);
        if (expected.Threw && actual.Threw)
            return Equals(expected.Exception, actual.Exception);
        return false;
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
            ReportMode = a.ReportMode & ~ReportMode.Warning & ~ReportMode.Labels & ~ReportMode.StackTrace
        };
    }
}
