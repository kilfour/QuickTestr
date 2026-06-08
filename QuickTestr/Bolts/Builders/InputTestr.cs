using QuickCheckr;
using QuickFuzzr;
using QuickTestr.Bolts.Builders.Oracle;
using QuickTestr.Bolts.Builders.Property;

namespace QuickTestr.Bolts.Builders;

/// <summary>
/// Configures how a generated input should be reduced, formatted, or evaluated.
/// Use to refine shrinking behavior before defining the actual assertion or oracle.
/// </summary>
public class InputTestr<T>(
    string testName,
    string fileName,
    bool useBuiltInReducers,
    CheckrOf<Case>[] formatters,
    FuzzrOf<T> fuzzr,
    Shrinker[] shrinkers)
{
    /// <summary>
    /// Adds a deliberation score used to guide shrinking.
    /// Use when smaller or simpler counterexamples depend on a domain-specific notion of progress.
    /// </summary>
    public DeliberatedTestr<T> Deliberate(Func<T, int> deliberation)
        => new(fuzzr, shrinkers, formatters, deliberation, null, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Adds a deliberation score and target used to guide shrinking.
    /// Use when shrinking should move toward a specific domain-aware goal instead of just lower scores.
    /// </summary>
    public DeliberatedTestr<T> Deliberate(Func<T, int> deliberation, int deliberationTarget)
        => new(fuzzr, shrinkers, formatters, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the property that must hold for generated inputs.
    /// Use for direct property-based testing where success is a boolean invariant.
    /// </summary>
    public TestrPropertyRunner<T> Assert(Func<T, bool> invariant)
        => new(fuzzr, shrinkers, formatters, invariant, null, null, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the expected result for oracle-style testing.
    /// Use when you want to compare a trusted model against another implementation.
    /// </summary>
    public OracleTestr<T, TResult> Expected<TResult>(Func<T, TResult> expected)
        => new(fuzzr, shrinkers, formatters, expected, null, null, testName, fileName, useBuiltInReducers);
}

/// <summary>
/// Configures how a generated input should be reduced, formatted, or evaluated.
/// Use to refine shrinking behavior before defining the actual assertion or oracle.
/// </summary>
public class InputTestr<T1, T2>(
    string testName,
    string fileName,
    bool useBuiltInReducers,
    CheckrOf<Case>[] formatters,
    FuzzrOf<T1> fuzzrOfT1,
    FuzzrOf<T2> fuzzrOfT2,
    Shrinker[] shrinkers)
{

    /// <summary>
    /// Adds a deliberation score used to guide shrinking.
    /// Use when smaller or simpler counterexamples depend on a domain-specific notion of progress.
    /// </summary>
    public DeliberatedTestr<T1, T2> Deliberate(Func<T1, T2, int> deliberation)
        => new(fuzzrOfT1, fuzzrOfT2, shrinkers, formatters, deliberation, null, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Adds a deliberation score and target used to guide shrinking.
    /// Use when shrinking should move toward a specific domain-aware goal instead of just lower scores.
    /// </summary>
    public DeliberatedTestr<T1, T2> Deliberate(Func<T1, T2, int> deliberation, int deliberationTarget)
        => new(fuzzrOfT1, fuzzrOfT2, shrinkers, formatters, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the property that must hold for generated inputs.
    /// Use for direct property-based testing where success is a boolean invariant.
    /// </summary>
    public TestrPropertyRunnerT2<T1, T2> Assert(Func<T1, T2, bool> invariant)
        => new(fuzzrOfT1, fuzzrOfT2, shrinkers, formatters, invariant, null, null, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the expected result for oracle-style testing.
    /// Use when you want to compare a trusted model against another implementation.
    /// </summary>
    public OracleTestrT2<T1, T2, TResult> Expected<TResult>(Func<T1, T2, TResult> expected)
        => new(fuzzrOfT1, fuzzrOfT2, shrinkers, formatters, expected, null, null, testName, fileName, useBuiltInReducers);
}