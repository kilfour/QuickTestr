namespace QuickTestr;

/// <summary>
/// Specifies how many independent searches are used when filling the vault.
/// Use to control how broadly QuickTestr looks for distinct failing cases.
/// </summary>
public record SearchCount(int NumberOfSearches);


public static class SearchCountExtensions
{
    /// <summary>
    /// Creates a <see cref="SearchCount"/> from the number of independent searches to run.
    /// </summary>
    public static SearchCount Searches(this int numberOfSearches) => new(numberOfSearches);
}
