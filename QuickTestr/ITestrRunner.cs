using QuickCheckr;
using QuickCheckr.UnderTheHood.Proceedings;
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
    CaseFile Run();

    /// <summary>
    /// Runs the Testr using the specified seed.
    /// Use when you want a reproducible execution of a known case.
    /// </summary>
    CaseFile Run(int seed);

    /// <summary>
    /// Runs the Testr using the specified number of runs.
    /// Use when you want to control how much search effort is spent.
    /// </summary>
    CaseFile Run(CheckrOfTRun.RunCount tries);

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
    /// Use when you want a representative set of different failures for the same Testr.
    /// </summary>
    void FillVault(
        SearchCount searchCount,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> classifyBy);

    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault using a custom policy.
    /// Use when you want to tune which cases are kept or ignored during vault filling.
    /// </summary>
    void FillVault(
        SearchCount searchCount,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> classifyBy,
        Func<VaultPolicy<TInput>, VaultPolicy<TInput>> policy);

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

/// <summary>
/// Specifies how many independent searches are used when filling the vault.
/// Use to control how broadly QuickTestr looks for distinct failing cases.
/// </summary>
public record SearchCount(int NumberOfSearches);
