using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.FilingCabinet;
using QuickCheckr.Protocol;


namespace QuickTestr.Bolts.Builders;

/// <summary>
/// Provides the shared execution and vault behavior for concrete Testr runners.
/// Use as the base implementation for property-based and oracle-based runner types.
/// </summary>
public abstract class TestrRunner<TInput> : ITestrRunner, ITestrRunner<TInput>
{
    /// <summary>
    /// Runs the Testr using the default number of runs.
    /// Use for the normal execution path when you do not need explicit configuration.
    /// </summary>
    [StackTraceHidden]
    public IRecord Run()
        => Run(100.Runs());

    /// <summary>
    /// Runs the Testr using the specified seed.
    /// Use when you want a reproducible execution of a known case.
    /// </summary>
    [StackTraceHidden]
    public IRecord Run(int seed)
        => GetCheckr().Run(seed, GetConfig());

    /// <summary>
    /// Runs the Testr using the specified number of runs.
    /// Use when you want to control how much search effort is spent.
    /// </summary>
    [StackTraceHidden]
    public IRecord Run(RunCount tries)
        => GetCheckr().Run(tries, GetConfig());

    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault.
    /// Use when you want a representative set of different failing inputs for the same Testr.
    /// Inputs are grouped by value and a limited set of failures (10) is retained.
    /// </summary>
    public IRecord FillVault(
        SearchCount searchCount,
        RunCount runs) =>
            FillVault(searchCount, runs, VaultPolicy<TInput>.Default);

    /// <summary>
    /// Searches for distinct failing cases and stores them in the vault using a custom policy.
    /// Use when you want to control how failures are grouped, limited, or skipped during vault filling.
    /// </summary>
    public IRecord FillVault(
        SearchCount searchCount,
        RunCount runs,
        VaultPolicy<TInput> policy) =>
            GetCheckr().Conduct(
                searchCount.NumberOfSearches.Investigations(),
                runs,
                1.ExecutionsPerRun(),
                AddFileAsToConfig(),
                new Directive
                {
                    ClassifyBy = (a) => policy.ClassifyBy(a.GetInput<TInput>("Input")),
                    MaxCaseFiles = policy.MaxStoredFailures,
                    Reject = policy.SkipWhen != null
                        ? a => policy.SkipWhen(a.GetInput<TInput>("Input")) : null
                });

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
