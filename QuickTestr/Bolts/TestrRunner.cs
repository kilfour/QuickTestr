using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;

namespace QuickTestr.Bolts;

public class TestrRunner<T>(
    FuzzrOf<T> fuzzr,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<T, bool> Invariant,
    Func<T, int>? Deliberation,
    int? DeliberationTarget,
    string testName,
    string fileName,
    bool UseBuiltInReducers) : BaseTestrRunner
{
    public override string TestName { get; } = testName;

    protected override CheckrOf<Case> GetCheckr() =>
        from showr in Showr.ForInput()
        from format in Combine.Checkrs(formatters)
        from input in Checkr.Input("Input", fuzzr, shrinkers)
        from run in Checkr.ActCarefully("Run", () => Invariant(input))
        from rethrow in Checkr.When(() => run.Threw, Checkr.Act("Rethrow", () => Invariant(input)))
        from expectation in Checkr.ExpectWhen(TestName, () => !run.Threw, () => run.Value)
        select Case.Closed;

    protected override Func<CheckrConfig, CheckrConfig> GetConfig()
    {
        return a => a with
        {
            FileAs = fileName,
            StyleGuide = TheTestr.DefaultStyleGuide,
            DeliberationPolicy = Deliberation == null ? null :
                a => a.InputsNamed<T>("Input", a => Deliberation(a)),
            DeliberationTarget = DeliberationTarget == null ? null : DeliberationTarget,
            ShrinkMode = UseBuiltInReducers ? a.ShrinkMode | ShrinkMode.Reduction : a.ShrinkMode,
            ReportMode = a.ReportMode & ~ReportMode.Warning & ~ReportMode.Labels & ~ReportMode.StackTrace
        };
    }
}
