using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickFuzzr;

namespace QuickTestr;

public class TestrOracleRunner<T, TResult>(
    FuzzrOf<T> fuzzr,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<T, TResult> Expected,
    Func<T, TResult> Actual,
    Func<T, int>? Deliberation,
    int? DeliberationTarget,
    string testName,
    bool UseBuiltInReducers) : ITestrRunner
{
    private CheckrOf<Case> GetCheckr() =>
        from showr in Showr.ForInput()
        from format in Combine.Checkrs(formatters)
        from input in Checkr.Input("Input", fuzzr, shrinkers)
        from expected in Checkr.ActCarefully("Expected", () => Expected(input))
        from actual in Checkr.ActCarefully("Actual", () => Actual(input))
        from traceExpectedValue in Checkr.TraceWhen("Expected", () => !expected.Threw, () => expected.Value)
        from traceExpectedException in Checkr.TraceWhen("Expected", () => expected.Threw, () => GetExceptionReport(expected.Exception!))
        from traceActualValue in Checkr.TraceWhen("Actual  ", () => !actual.Threw, () => actual.Value)
        from traceActualException in Checkr.TraceWhen("Actual  ", () => actual.Threw, () => GetExceptionReport(actual.Exception!))
        from expectation in Checkr.Expect(testName, () => CheckResults(expected, actual))
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

    [StackTraceHidden]
    public CaseFile Run()
        => Run(100.Runs());

    [StackTraceHidden]
    public CaseFile Run(int seed)
        => GetCheckr().Run(seed, GetConfig());

    [StackTraceHidden]
    public CaseFile Run(CheckrOfTRun.RunCount tries)
        => GetCheckr().Run(tries, GetConfig());

    private Func<CheckrConfig, CheckrConfig> GetConfig()
    {
        return a => a with
        {
            StyleGuide = TheTestr.DefaultStyleGuide,
            DeliberationPolicy = Deliberation == null ? null :
                a => a.InputsNamed<T>("Input", a => Deliberation(a)),
            DeliberationTarget = DeliberationTarget == null ? null : DeliberationTarget,
            ShrinkMode = UseBuiltInReducers ? a.ShrinkMode | ShrinkMode.Reduction : a.ShrinkMode,
            ReportMode = a.ReportMode & ~ReportMode.Warning & ~ReportMode.Labels & ~ReportMode.StackTrace
        };
    }
}
