using QuickCheckr;
using QuickFuzzr;

namespace QuickTestr;


public static class Testr
{
    public static TestrBuilder0 Named(string name) => new(name);
}

public class TestrBuilder0(string testName)
{
    private string fileName = string.Empty;
    public TestrBuilder0 StoreCaseFiles() { fileName = testName; return this; }

    public TestrBuilder1<T> For<T>(FuzzrOf<T> fuzzr, params Shrinker[] shrinkers)
        => new(fuzzr, shrinkers, testName, fileName);
}

public class TestrBuilder1<T>(FuzzrOf<T> fuzzr, Shrinker[] shrinkers, string testName, string fileName)
{
    private bool useBuiltInReducers = true;
    private CheckrOf<Case>[] formatters = [];
    public TestrBuilder1<T> DisableValueReduction() { useBuiltInReducers = false; return this; }
    public TestrBuilder1<T> Format(CheckrOf<Case>[] formatters) { this.formatters = formatters; return this; }
    public TestrBuilder2<T> Deliberate(Func<T, int> deliberation)
        => new(fuzzr, shrinkers, formatters, deliberation, null, testName, fileName, useBuiltInReducers);
    public TestrBuilder2<T> Deliberate(Func<T, int> deliberation, int deliberationTarget)
        => new(fuzzr, shrinkers, formatters, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);
    public TestrRunner<T> Assert(Func<T, bool> invariant)
        => new(fuzzr, shrinkers, formatters, invariant, null, null, testName, fileName, useBuiltInReducers);
    public TestrOracleBuilder<T, TResult> Expected<TResult>(Func<T, TResult> expected)
        => new(fuzzr, shrinkers, formatters, expected, null, null, testName, fileName, useBuiltInReducers);
}

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
    public TestrRunner<T> Assert(Func<T, bool> invariant)
        => new(fuzzr, shrinkers, formatters, invariant, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);
    public TestrOracleBuilder<T, TResult> Expected<TResult>(Func<T, TResult> expected)
        => new(fuzzr, shrinkers, formatters, expected, deliberation, deliberationTarget, testName, fileName, useBuiltInReducers);
}
