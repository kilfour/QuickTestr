using QuickCheckr;
using QuickFuzzr;
using QuickTestr.Bolts.Runners;

namespace QuickTestr.Bolts.Builders;

/// <summary>
/// Completes a Testr after deliberation has been configured.
/// Use to finish the definition with either a property assertion or an oracle comparison.
/// </summary>
public class DeliberatedTestr<T>(
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
    public TestrPropertyRunner<T> Assert(Func<T, bool> invariant)
        => new(fuzzr, shrinkers, formatters, invariant, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);

    /// <summary>
    /// Defines the expected result for oracle-style testing.
    /// Use when you want to compare a trusted model against another implementation after adding deliberation settings.
    /// </summary>
    public OracleTestr<T, TResult> Expected<TResult>(Func<T, TResult> expected)
        => new(fuzzr, shrinkers, formatters, expected, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);
}
