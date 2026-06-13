using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickTestr.Bolts.ClerksOffice;

namespace QuickTestr.Bolts.Builders.Property;

/// <summary>
/// Runs a property-based Testr against generated inputs.
/// Use for Testrs defined with a boolean invariant through Assert.
/// </summary>
public class TestrPropertyRunnerT2<TInput1, TInput2>(
    FuzzrOf<TInput1> fuzzrOfT1,
    FuzzrOf<TInput2> fuzzrOfT2,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<TInput1, TInput2, bool> Invariant,
    Func<TInput1, TInput2, int>? Deliberation,
    int? DeliberationTarget,
    string testName,
    string fileName,
    bool UseBuiltInReducers) : TestrRunner<(TInput1, TInput2)>
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
        from run in Checkr.ActCarefully("Run", () => Invariant(input.Item1, input.Item2))
        from rethrow in Checkr.When(() => run.Threw, Checkr.Act("Rethrow", () => Invariant(input.Item1, input.Item2)))
        from expectation in Checkr.ExpectWhen(TestName, () => !run.Threw, () => run.Value)
        select Case.Closed;

    protected override Func<CheckrConfig, CheckrConfig> GetConfig()
    {
        return a => a with
        {
            FileAs = fileName,
            Clerk = new PropertyClerk(),
            Deliberation = Deliberation != null
                ? new Deliberation(a => a.InputsNamed<(TInput1, TInput2)>("Input", a => Deliberation(a.Item1, a.Item2)), DeliberationTarget)
                : null,
            ShrinkMode = UseBuiltInReducers ? a.ShrinkMode | ShrinkMode.Reduction : a.ShrinkMode,
            ReportMode = a.ReportMode & ~ReportMode.Labels & ~ReportMode.StackTrace
        };
    }
}
