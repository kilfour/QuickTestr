using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickCheckr.UnderTheHood.Proceedings;

namespace QuickTestr.Bolts.Modelr;

/// <summary>
/// Completes a model-based Testr after operations and an initial observation are defined.
/// Use when adding more observations or running the configured model-based check.
/// </summary>
public sealed class WithOperations<T, U>(
    string testName,
    string fileName,
    bool useBuiltInReducers,
    CheckrOf<Case>[] formatters,
    Func<T> model,
    Func<U> sut,
    List<Func<T, U, CheckrOf<(Func<bool> condition, CheckrOf<Case> checkr)>>> operations,
    Observation<T, U> observation) : IModelrRunner
{
    /// <summary>
    /// Gets the display name of this Testr.
    /// Use when you need the configured name for reporting or storage.
    /// </summary>
    public string TestName { get; } = testName;

    /// <summary>
    /// Adds another observation to the model-based Testr.
    /// Use when several invariants should hold after each generated operation.
    /// </summary>
    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe)
    {
        observations.Add(Observation.From(label, observe));
        return this;
    }

    /// <summary>
    /// Adds another observation with custom trace output to the model-based Testr.
    /// Use when a failing invariant needs targeted diagnostic values in the report.
    /// </summary>
    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe, Func<ITracer<T, U>, ITracer<T, U>> trace)
    {
        observations.Add(Observation.From(label, observe, ((Tracer<T, U>)trace(new Tracer<T, U>())).TraceCheckr));
        return this;
    }

    /// <summary>
    /// Runs the model-based Testr using the default execution settings.
    /// Use for the normal execution path when you do not need explicit run control.
    /// </summary>
    [StackTraceHidden]
    public CaseFile Run()
        => GetCheckr().Run(10.Runs(), 50.ExecutionsPerRun(), GetConfig());

    /// <summary>
    /// Runs the model-based Testr using the specified run and execution counts.
    /// Use when you want to control how much stateful exploration is performed.
    /// </summary>
    [StackTraceHidden]
    public CaseFile Run(CheckrOfTRun.RunCount runs, CheckrOfTRun.ExecutionCount executionsPerRun)
        => GetCheckr().Run(runs, executionsPerRun, GetConfig());

    /// <summary>
    /// Runs the model-based Testr using the specified seed.
    /// Use when you want to reproduce a known stateful execution path.
    /// </summary>
    [StackTraceHidden]
    public CaseFile Run(int seed, CheckrOfTRun.ExecutionCount executionsPerRun)
        => GetCheckr().Run(seed, executionsPerRun, GetConfig());

    private Func<CheckrConfig, CheckrConfig> GetConfig()
    {
        return a => a with
        {
            FileAs = fileName,
            // StyleGuide = TheTestr.DefaultStyleGuide,
            // DeliberationPolicy = Deliberation == null ? null :
            //     a => a.InputsNamed<TInput>("Input", a => Deliberation(a)),
            // DeliberationTarget = DeliberationTarget == null ? null : DeliberationTarget,
            ShrinkMode = useBuiltInReducers ? a.ShrinkMode | ShrinkMode.Reduction : a.ShrinkMode,
            ReportMode = a.ReportMode & ~ReportMode.Labels & ~ReportMode.StackTrace
        };
    }

    private readonly Func<T> model = model;
    private readonly Func<U> sut = sut;
    private readonly List<Func<T, U, CheckrOf<(Func<bool> condition, CheckrOf<Case> checkr)>>> operations = operations;
    private readonly List<Observation<T, U>> observations = [observation];

    private CheckrOf<Case> GetCheckr()
    {
        var checkr =
            from m in Trackr.Stashed(model)
            from s in Trackr.Stashed(sut)
            from showr in Showr.ForInput()
            from format in Combine.Checkrs(formatters)
            from ops in Checkr.OneOfWhen([.. operations.Select(a => a(m, s))])
            from obs in Combine.Checkrs(observations.Select(a => a.Observe(m, s)))
            select Case.Closed;
        return checkr;
    }
}
