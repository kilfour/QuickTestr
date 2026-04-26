using QuickCheckr;
using QuickFuzzr;

namespace QuickTestr.Bolts;

/// <summary>
/// Starts the fluent configuration of a Testr.
/// Use to opt into case-file storage or move on to input generation.
/// </summary>
public class TestrBuilder0(string testName)
{
    private string fileName = string.Empty;

    /// <summary>
    /// Persists case files for this Testr under its test name.
    /// Use when you want to inspect or clean up stored cases later through the vault workflow.
    /// </summary>
    public TestrBuilder0 StoreCaseFiles() { fileName = testName; return this; }

    /// <summary>
    /// Selects the input generator and optional custom shrinkers for this Testr.
    /// Use to define the values QuickTestr should explore and how they should shrink.
    /// </summary>
    public TestrBuilder1<T> For<T>(FuzzrOf<T> fuzzr, params Shrinker[] shrinkers)
        => new(fuzzr, shrinkers, testName, fileName);
}

/// <summary>
/// Configures how a generated input should be reduced, formatted, or evaluated.
/// Use to refine shrinking behavior before defining the actual assertion or oracle.
/// </summary>
public class TestrBuilder1<T>(FuzzrOf<T> fuzzr, Shrinker[] shrinkers, string testName, string fileName)
{
    private bool useBuiltInReducers = true;
    private CheckrOf<Case>[] formatters = [];

    /// <summary>
    /// Disables the built-in value reduction step during shrinking.
    /// Use when structural shrinking is enough or value reduction pushes examples the wrong way.
    /// </summary>
    public TestrBuilder1<T> DisableValueReduction() { useBuiltInReducers = false; return this; }

    /// <summary>
    /// Adds custom formatters to the report for the generated input.
    /// Use when the default rendering does not explain the failing example clearly enough.
    /// </summary>
    public TestrBuilder1<T> Format(CheckrOf<Case>[] formatters) { this.formatters = formatters; return this; }

    /// <summary>
    /// Adds a deliberation score used to guide shrinking.
    /// Use when smaller or simpler counterexamples depend on a domain-specific notion of progress.
    /// </summary>
    public TestrBuilder2<T> Deliberate(Func<T, int> deliberation)
        => new(fuzzr, shrinkers, formatters, deliberation, null, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Adds a deliberation score and target used to guide shrinking.
    /// Use when shrinking should move toward a specific domain-aware goal instead of just lower scores.
    /// </summary>
    public TestrBuilder2<T> Deliberate(Func<T, int> deliberation, int deliberationTarget)
        => new(fuzzr, shrinkers, formatters, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the property that must hold for generated inputs.
    /// Use for direct property-based testing where success is a boolean invariant.
    /// </summary>
    public TestrRunner<T> Assert(Func<T, bool> invariant)
        => new(fuzzr, shrinkers, formatters, invariant, null, null, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the expected result for oracle-style testing.
    /// Use when you want to compare a trusted model against another implementation.
    /// </summary>
    public TestrOracleBuilder<T, TResult> Expected<TResult>(Func<T, TResult> expected)
        => new(fuzzr, shrinkers, formatters, expected, null, null, testName, fileName, useBuiltInReducers);
}

/// <summary>
/// Completes a Testr after deliberation has been configured.
/// Use to finish the definition with either a property assertion or an oracle comparison.
/// </summary>
public class TestrBuilder2<T>(
    FuzzrOf<T> fuzzr,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<T, int>? deliberation,
    int? deliberationTarget,
    string testName,
    string fileName,
    bool useBuiltInReducers)
{
    /// <summary>
    /// Defines the property that must hold for generated inputs.
    /// Use for direct property-based testing after adding deliberation settings.
    /// </summary>
    public TestrRunner<T> Assert(Func<T, bool> invariant)
        => new(fuzzr, shrinkers, formatters, invariant, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the expected result for oracle-style testing.
    /// Use when you want to compare a trusted model against another implementation after adding deliberation settings.
    /// </summary>
    public TestrOracleBuilder<T, TResult> Expected<TResult>(Func<T, TResult> expected)
        => new(fuzzr, shrinkers, formatters, expected, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);
}
