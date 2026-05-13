using QuickCheckr;
using QuickFuzzr;
using QuickTestr.Bolts.Runners;

namespace QuickTestr.Bolts.Builders.Tuple2;

/// <summary>
/// Configures how a generated input should be reduced, formatted, or evaluated.
/// Use to refine shrinking behavior before defining the actual assertion or oracle.
/// </summary>
public class InputTestr<T1, T2>(FuzzrOf<T1> fuzzrOfT1, FuzzrOf<T2> fuzzrOfT2, Shrinker[] shrinkers, string testName, string fileName)
{
    private bool useBuiltInReducers = true;
    private CheckrOf<Case>[] formatters = [];

    /// <summary>
    /// Disables the built-in value reduction step during shrinking.
    /// Use when structural shrinking is enough or value reduction pushes examples the wrong way.
    /// </summary>
    public InputTestr<T1, T2> DisableValueReduction() { useBuiltInReducers = false; return this; }

    /// <summary>
    /// Adds custom formatters to the report for the generated input.
    /// Use when the default rendering does not explain the failing example clearly enough.
    /// </summary>
    public InputTestr<T1, T2> Format(CheckrOf<Case>[] formatters) { this.formatters = formatters; return this; }

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
