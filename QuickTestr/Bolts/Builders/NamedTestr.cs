using QuickCheckr;
using QuickFuzzr;
using QuickTestr.Bolts.Modelr;

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

    /// <summary>
    /// Selects two input generators and optional custom shrinkers for this Testr.
    /// Use to define pairs of values QuickTestr should explore together and how they should shrink.
    /// </summary>
    public InputTestr<T1, T2> For<T1, T2>(FuzzrOf<T1> fuzzrOfT1, FuzzrOf<T2> fuzzrOfT2, params Shrinker[] shrinkers)
        => new(fuzzrOfT1, fuzzrOfT2, shrinkers, testName, fileName);

    /// <summary>
    /// Starts a model-based Testr from a trusted model instance.
    /// Use when you want to compare a stateful system under test against a reference implementation.
    /// </summary>
    public WithModel<T> Model<T>(Func<T> model) => new(testName, fileName, model);
}
