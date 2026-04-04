using QuickCheckr;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickCheckr.UnderTheHood.Proceedings.ClerksOffice;
using QuickCheckr.UnderTheHood.Proceedings.Depositions;
using QuickCheckr.UnderTheHood.Proceedings.Depositions.Failure;
using QuickPulse;

namespace QuickTestr;

public static class TheTestr
{
    private readonly static Flow<Flow> space =
        Pulse.Trace(" ");
    private static readonly Flow<Flow> newLine =
        Pulse.Trace(Environment.NewLine);

    private static Flow<Flow> LineOf(int length)
        => Pulse.Trace(new string('-', length));

    private readonly static Flow<Flow> line =
        space.Then(LineOf(60)).Then(newLine);

    private static Flow<Flow> WarningFlow(WarningDeposition warning)
        => newLine.Then(Pulse.Trace($"   - WARNING: {warning.Value}"));

    private static Flow<Flow> InputFlow(InputDeposition input) =>
        from _1 in Pulse.Trace($"    Input = {input.Value}").Then(newLine)
        from _2 in Pulse.When(input.Redux.HasValue,
            Pulse.Trace($"    Redux = {input.Redux.Value}").Then(newLine))
        from _3 in Pulse.When(input.Original.HasValue,
            newLine
                .Then(Pulse.Trace("  Original:"))
                .Then(newLine)
                .Then(Pulse.Trace($"    {input.Original.Value!}")))
        select Flow.Continue;

    private static Flow<Flow> ExecutionFlow(ExecutionDeposition execution) =>
        from _1 in newLine
        from _2 in Pulse.ToFlow(InputFlow, execution.InputDepositions)
        from _3 in Pulse.ToFlow(WarningFlow, execution.GetWarningDepositionsForReport())
        from _4 in newLine.Then(space).Then(LineOf(60))
        select Flow.Continue;

    private static Flow<Flow> FailedExpectationFlow(FailedExpectationDeposition failure)
        => Pulse.Trace($"  {failure.FailedExpectation}").Then(newLine);

    private static Flow<Flow> ExceptionFlow(FailedExceptionDeposition failure)
        => Pulse.Trace($"  {failure.Message}").Then(newLine);

    private static Flow<Flow> FailureFlow(FailureDeposition failure) =>
        from _1 in Pulse.OnType((FailedExpectationDeposition a) => FailedExpectationFlow(a), () => failure)
        from _2 in Pulse.OnType((FailedExceptionDeposition a) => ExceptionFlow(a), () => failure)
        select Flow.Continue;

    private static string Pluralize(int count, string str) =>
        count > 1 ? $"{str}s" : str;

    private static Flow<Flow> EvidenceFlow(CaseFile caseFile) =>
        from _1 in line
        from _2 in Pulse.ToFlow(FailureFlow, caseFile.FailureDeposition)
        from _3 in Pulse.Trace($"  Seed: {caseFile.Seed}").Then(newLine)
        from _4 in line
        from _5 in Pulse.Trace("  Falsified:")
        from _6 in Pulse.ToFlow(ExecutionFlow, caseFile.ExecutionDepositions)
        select Flow.Continue;

    private static Flow<Flow> SummaryFlow(CaseSummary summary) =>
        Pulse.Trace($" {summary.NumberOfRuns} {Pluralize(summary.NumberOfRuns, "Run")}{Environment.NewLine}");

    public static Flow<Flow> StyleGuide(CaseFile caseFile) =>
        from _1 in Pulse.ToFlowIf(caseFile.HasEvidence, EvidenceFlow, () => caseFile)
        from _2 in Pulse.OnType((CaseSummary a) => SummaryFlow(a), () => caseFile)
        select Flow.Continue;
}