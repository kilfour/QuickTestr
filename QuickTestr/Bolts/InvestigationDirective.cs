namespace QuickTestr.Bolts;

public record InvestigationDirective<TInput>(int? MaxCaseFiles = 5, Func<TInput, bool>? RejectWhen = null);
