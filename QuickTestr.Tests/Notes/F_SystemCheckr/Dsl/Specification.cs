using QuickCheckr;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;

public record Specification(
    CheckrOf<(Func<bool>, CheckrOf<Case>)> Checkr);