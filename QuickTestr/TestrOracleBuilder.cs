using QuickCheckr;
using QuickFuzzr;

namespace QuickTestr;

public class TestrOracleBuilder<T, TResult>(
    FuzzrOf<T> fuzzr,
    Shrinker[] shrinkers,
    CheckrOf<Case>[] formatters,
    Func<T, TResult> expected,
    Func<T, int>? deliberation,
    int? deliberationTarget,
    string testName,
    bool useBuiltInReducers)
{
    public TestrOracleRunner<T, TResult> Actual(Func<T, TResult> actual)
        => new(fuzzr, shrinkers, formatters, expected, actual, deliberation, deliberationTarget, testName, useBuiltInReducers);
}
