using QuickCheckr;
using QuickCheckr.FilingCabinet;
using QuickCheckr.Protocol.Custodians;
using QuickCheckr.UnderTheHood;

namespace QuickTestr.Bolts.Builders.ModelBased;

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
    void Run();

    /// <summary>
    /// Runs the model-based Testr using the specified run and execution counts.
    /// Use when you want to control how much stateful exploration is performed.
    /// </summary>
    void Run(RunCount runs, ExecutionCount executionsPerRun);

    /// <summary>
    /// Runs the model-based Testr using the specified seed.
    /// Use when you want to reproduce a known stateful execution path.
    /// </summary>
    void Run(int seed, ExecutionCount executionsPerRun);

    /// <summary>
    /// Persists case files for this Testr under its test name.
    /// Use when you want to inspect or clean up stored cases later through the vault workflow.
    /// </summary>
    IModelrRunner StoreCaseFiles(ICustodian? custodian = null);
}
