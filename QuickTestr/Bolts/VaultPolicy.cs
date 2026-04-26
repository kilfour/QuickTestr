namespace QuickTestr.Bolts;

/// <summary>
/// Configures which cases are kept while filling the vault.
/// Use to limit stored cases or skip inputs that are not worth persisting.
/// </summary>
public record VaultPolicy<TInput>(int? MaxStoredCases = 5, Func<TInput, bool>? SkipWhen = null);
