using QuickCheckr;
using QuickCheckr.FilingCabinet;

namespace QuickTestr.Bolts.Modelr;

/// <summary>
/// Runs a model-based Testr through a non-generic handle.
/// Use when you want to store or pass around a configured model-based runner without its type arguments.
/// </summary>
public interface IModelrRunner
{
    /// <summary>
    /// Runs the model-based Testr using the default execution settings.
    /// Use for the normal execution path when you do not need explicit run control.
    /// </summary>
    IRecord Run();

    /// <summary>
    /// Runs the model-based Testr using the specified run and execution counts.
    /// Use when you want to control how much stateful exploration is performed.
    /// </summary>
    IRecord Run(CheckrOfTRun.RunCount runs, CheckrOfTRun.ExecutionCount executionsPerRun);

    /// <summary>
    /// Runs the model-based Testr using the specified seed.
    /// Use when you want to reproduce a known stateful execution path.
    /// </summary>
    IRecord Run(int seed, CheckrOfTRun.ExecutionCount executionsPerRun);
}
