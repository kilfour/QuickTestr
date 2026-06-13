using QuickCheckr;
using QuickCheckr.FilingCabinet;
using QuickCheckr.UnderTheHood;
using QuickTestr.Bolts;

namespace QuickTestr;

/// <summary>
/// Runs a Testr and exposes advanced vault workflows.
/// Use as the common non-generic handle for storing a configured Testr.
/// </summary>
public interface ITestrRunner
{
    /// <summary>
    /// Runs the Testr using the default number of runs.
    /// Use for the normal execution path when you do not need explicit configuration.
    /// </summary>
    ConfiguredCheckr Run();

    /// <summary>
    /// Runs the Testr using the specified seed.
    /// Use when you want a reproducible execution of a known case.
    /// </summary>
    ConfiguredCheckr Run(int seed);

    /// <summary>
    /// Runs the Testr using the specified number of runs.
    /// Use when you want to control how much search effort is spent.
    /// </summary>
    ConfiguredCheckr Run(RunCount tries);

    /// <summary>
    /// Re-enters the typed vault workflow for this Testr.
    /// Use when the runner is stored non-generically but you want to fill or inspect the vault.
    /// </summary>
    ITestrRunner<TInput> WithVault<TInput>();
}

/// <summary>
/// Exposes typed vault workflows for a Testr.
/// Use when you need input-aware vault operations such as classification.
/// </summary>
public interface ITestrRunner<TInput>
{
    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault.
    /// Use when you want a representative set of different failing inputs for the same Testr.
    /// Inputs are grouped by value and a limited set of failures (10) is retained.
    /// </summary>
    ConfiguredCheckr FillVault(
        SearchCount searchCount,
        RunCount runs);

    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault using a custom policy.
    /// Use when you want to control how failures are grouped, limited, or skipped during vault filling.
    /// </summary>
    ConfiguredCheckr FillVault(
        SearchCount searchCount,
        RunCount runs,
        VaultPolicy<TInput> policy);

    /// <summary>
    /// Re-runs the stored vault cases and reports which ones still fail.
    /// Use to review persisted seeds after code changes or fixes.
    /// </summary>
    void InspectVault();

    /// <summary>
    /// Removes or closes vault cases that no longer reproduce.
    /// Use to keep the vault focused on still-relevant failures.
    /// </summary>
    void CleanupVault();
}
