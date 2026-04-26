namespace QuickTestr;

/// <summary>
/// Specifies how many independent searches are used when filling the vault.
/// Use to control how broadly QuickTestr looks for distinct failing cases.
/// </summary>
public record SearchCount(int NumberOfSearches);
