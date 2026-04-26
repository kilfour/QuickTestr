using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood.Proceedings;


namespace QuickTestr.Bolts;

/// <summary>
/// Provides the shared execution and vault behavior for concrete Testr runners.
/// Use as the base implementation for property-based and oracle-based runner types.
/// </summary>
public abstract class BaseTestrRunner<TInput> : ITestrRunner, ITestrRunner<TInput>
{
    /// <summary>
    /// Runs the Testr using the default number of runs.
    /// Use for the normal execution path when you do not need explicit configuration.
    /// </summary>
    [StackTraceHidden]
    public CaseFile Run()
        => Run(100.Runs());

    /// <summary>
    /// Runs the Testr using the specified seed.
    /// Use when you want a reproducible execution of a known case.
    /// </summary>
    [StackTraceHidden]
    public CaseFile Run(int seed)
        => GetCheckr().Run(seed, GetConfig());

    /// <summary>
    /// Runs the Testr using the specified number of runs.
    /// Use when you want to control how much search effort is spent.
    /// </summary>
    [StackTraceHidden]
    public CaseFile Run(CheckrOfTRun.RunCount tries)
        => GetCheckr().Run(tries, GetConfig());

    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault using a custom policy.
    /// Use when you want to tune which cases are kept or ignored during vault filling.
    /// </summary>
    public void FillVault(
        SearchCount searchCount,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> classifyBy,
        Func<VaultPolicy<TInput>, VaultPolicy<TInput>> policy)
    {
        var configuredPolicy = policy(new VaultPolicy<TInput>(5, null));
        GetCheckr().GatherEvidence(
                searchCount.NumberOfSearches.Investigations(),
                runs,
                1.ExecutionsPerRun(),
                AddFileAsToConfig(),
                (a) => classifyBy(a.GetInput<TInput>("Input")),
                configuredPolicy.MaxStoredCases,
                configuredPolicy.SkipWhen != null ? a => configuredPolicy.SkipWhen(a.GetInput<TInput>("Input")) : null);
    }

    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault.
    /// Use when you want a representative set of different failures for the same Testr.
    /// </summary>
    public void FillVault(
        SearchCount searchCount,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> classifyBy)
            => FillVault(searchCount, runs, classifyBy, a => a);

    /// <summary>
    /// Re-runs the stored vault cases and reports which ones still fail.
    /// Use to review persisted seeds after code changes or fixes.
    /// </summary>
    public void InspectVault()
        => GetCheckr().ReviewColdCases(AddFileAsToConfig());

    /// <summary>
    /// Removes or closes vault cases that no longer reproduce.
    /// Use to keep the vault focused on still-relevant failures.
    /// </summary>
    public void CleanupVault()
        => GetCheckr().CloseResolvedColdCases(AddFileAsToConfig());

    private Func<CheckrConfig, CheckrConfig> AddFileAsToConfig()
        => a => GetConfig()(a) with { FileAs = TestName };

    protected abstract CheckrOf<Case> GetCheckr();
    protected abstract Func<CheckrConfig, CheckrConfig> GetConfig();

    /// <summary>
    /// Re-enters the typed vault workflow for this Testr.
    /// Use when the runner is stored non-generically but you want to fill or inspect the vault.
    /// </summary>
    public ITestrRunner<T> WithVault<T>()
    {
        if (this is ITestrRunner<T> typed)
            return typed;
        throw new InvalidOperationException(
            $"This Testr expects input of type '{typeof(TInput).Name}', not '{typeof(T).Name}'.");
    }

    /// <summary>
    /// Gets the display name of this Testr.
    /// Use when you need the configured name for reporting or storage.
    /// </summary>
    public abstract string TestName { get; }
}
