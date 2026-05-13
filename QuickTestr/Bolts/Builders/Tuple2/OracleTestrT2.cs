using QuickCheckr;
using QuickFuzzr;
using QuickTestr.Bolts.Runners;

namespace QuickTestr.Bolts.Builders;

/// <summary>
/// Completes an oracle-based Testr after the expected behavior has been defined.
/// Use to provide the implementation that should match the trusted model.
/// </summary>
public class OracleTestrT2<TInput1, TInput2, TResult>(
    FuzzrOf<TInput1> fuzzrOfT1,
    FuzzrOf<TInput2> fuzzrOfT2,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<TInput1, TInput2, TResult> expected,
    Func<TInput1, TInput2, int>? deliberation,
    int? deliberationTarget,
    string testName,
    string fileName,
    bool useBuiltInReducers)
{
    /// <summary>
    /// Defines the implementation that should match the expected result.
    /// Use to finish an oracle-style Testr and obtain a runnable comparison.
    /// </summary>
    public TestrOracleRunnerT2<TInput1, TInput2, TResult> Actual(Func<TInput1, TInput2, TResult> actual)
        => new(fuzzrOfT1, fuzzrOfT2, shrinkers, formatters, expected, actual, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);
}
