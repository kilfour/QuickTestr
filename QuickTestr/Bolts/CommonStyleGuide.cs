using QuickCheckr.FilingCabinet;
using QuickCheckr.FilingCabinet.Depositions;
using QuickCheckr.FilingCabinet.Depositions.Failure;
using QuickPulse;

namespace QuickTestr.Bolts;

public static class CommonStyleGuide
{

    /// <summary>
    /// Formats a case file using the property-based QuickTestr report style.
    /// Use for standard property-based or oracle-based Testr output.
    /// </summary>
    public static Flow<Flow> Render(IRecord record, Func<ExecutionDeposition, Flow<Flow>> renderExecution) =>
        Pulse.Prime(() => renderExecution).Dissipate()
            .OnType((Findings a) => Findings(a), () => record)
            .OnType((CaseFile a) => CaseFile(a), () => record);

    public static Flow<Flow> Findings(Findings summary) =>
        Style
            .DrawTopLine()
            .OnNewLine()
            .Trace(summary.NumberOfRuns)
            .Space()
            .Trace(Style.Pluralize(summary.NumberOfRuns, "Run"))
            .DrawLine();

    private static Flow<Flow> CaseFile(CaseFile caseFile) =>
        Style
            .DrawTopLine()
            .ToFlow(FailureFlow, caseFile.FailureDeposition)
            .OnNewLine().Indent(1).Trace($"Seed: {caseFile.Seed}")
            .DrawLine()
            .OnNewLine().Indent(1).Trace("Falsified:")
            .ToFlow(ExecutionFlow, caseFile.ExecutionDepositions);

    public static Flow<Flow> FailureFlow(FailureDeposition failure) =>
        Pulse
            .OnType((FailedExpectationDeposition a) => FailedExpectationFlow(a), () => failure)
            .OnType((FailedExceptionDeposition a) => ExceptionFlow(a), () => failure);

    private static Flow<Flow> FailedExpectationFlow(FailedExpectationDeposition failure)
        => Style.OnNewLine().Indent(1).Trace(failure.FailedExpectation);

    private static Flow<Flow> ExceptionFlow(FailedExceptionDeposition failure)
        => Style.OnNewLine().Indent(1).Trace(failure.Message);

    private static Flow<Flow> ExecutionFlow(ExecutionDeposition execution) =>
        from executionFlow in Pulse.Draw<Func<ExecutionDeposition, Flow<Flow>>>()
        from _ in
            Pulse
            .ToFlow(executionFlow, execution)
            .DrawLine()
        select Flow.Continue;

    public static Flow<Flow> WarningsFlow(IEnumerable<WarningDeposition> warnings)
        => Pulse
            .When(warnings.Any(), Style.NewLine())
            .ToFlow(WarningFlow, warnings);

    private static Flow<Flow> WarningFlow(WarningDeposition warning)
        => Style
            .OnNewLine()
            .Indent(1)
            .Trace("WARNING:")
            .Space()
            .Trace(warning.Value);
}

