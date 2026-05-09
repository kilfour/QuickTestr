using QuickCheckr;
using QuickFuzzr;

namespace QuickTestr.Bolts.Builders;

/// <summary>
/// Starts the fluent configuration of a Testr.
/// Use to opt into case-file storage or move on to input generation.
/// </summary>
public class NamedTestr(string testName)
{
    private string fileName = string.Empty;

    /// <summary>
    /// Persists case files for this Testr under its test name.
    /// Use when you want to inspect or clean up stored cases later through the vault workflow.
    /// </summary>
    public NamedTestr StoreCaseFiles() { fileName = testName; return this; }

    /// <summary>
    /// Selects the input generator and optional custom shrinkers for this Testr.
    /// Use to define the values QuickTestr should explore and how they should shrink.
    /// </summary>
    public InputTestr<T> For<T>(FuzzrOf<T> fuzzr, params Shrinker[] shrinkers)
        => new(fuzzr, shrinkers, testName, fileName);
}
