using QuickCheckr;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickCheckr.UnderTheHood.Proceedings.ClerksOffice;
using QuickCheckr.UnderTheHood.Proceedings.Depositions;
using QuickCheckr.UnderTheHood.Proceedings.Depositions.Failure;
using QuickPulse;

namespace QuickTestr.Bolts;

/// <summary>
/// Selects the report style used by QuickTestr.
/// Use to distinguish between default property reports and oracle comparison reports.
/// </summary>
public enum Styles { Default, Oracle }

/// <summary>
/// Provides the built-in report style guides used by QuickTestr.
/// Use when you want the standard compact formatting for property-based or oracle-based cases.
/// </summary>
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

    private static Flow<Flow> TraceFlow(TraceDeposition trace)
        => newLine.Then(Pulse.Trace($"        {trace.Label} = {trace.Value}"));

    private static Flow<Flow> TracesFlow(List<TraceDeposition> traces) =>
        from _1 in Pulse.When(traces.Count > 0, newLine.Then(Pulse.Trace("      Observed:")))
        from _2 in Pulse.ToFlow(a => TraceFlow(a), traces)
        select Flow.Continue;

    private static Flow<Flow> WarningFlow(WarningDeposition warning)
        => newLine.Then(Pulse.Trace($"   - WARNING: {warning.Value}"));

    /// <summary>
    /// Represents the rendered input payload for an oracle-style report.
    /// Use internally when formatting expected and actual observations for the same input.
    /// </summary>
    public record OracleInput(InputDeposition Input, List<TraceDeposition> Traces, List<TraceDeposition> FinalTraces);
    private static Flow<Flow> OracleInputFlow(OracleInput oracleInput) =>
        from _1 in newLine.Then(Pulse.Trace($"    Input = {oracleInput.Input.Value}"))
        from _2 in Pulse.ToFlow(TracesFlow, oracleInput.Traces)
        from _3 in Pulse.When(oracleInput.Input.Redux.HasValue, (
            from _3_1 in newLine.Then(newLine).Then(Pulse.Trace($"    Redux = {oracleInput.Input.Redux.Value}"))
            from _3_2 in Pulse.ToFlow(TracesFlow, oracleInput.FinalTraces)
            select Flow.Continue))
        from _4 in Pulse.When(oracleInput.Input.Original.HasValue && !Equals(oracleInput.Input.Value, oracleInput.Input.Original.Value),
            newLine.Then(newLine).Then(Pulse.Trace("  Original:"))
                .Then(newLine)
                .Then(Pulse.Trace($"    {oracleInput.Input.Original.Value!}")))
        select Flow.Continue;

    private static Flow<Flow> InputFlow(InputDeposition input) =>
        from _1 in Pulse.Trace($"    Input = {input.Value}")
        from _2 in Pulse.When(input.Redux.HasValue,
            newLine.Then(Pulse.Trace($"    Redux = {input.Redux.Value}")))
        from _5 in Pulse.When(input.Original.HasValue && !Equals(input.Value, input.Original.Value),
            newLine.Then(newLine).Then(Pulse.Trace("  Original:"))
                .Then(newLine)
                .Then(Pulse.Trace($"    {input.Original.Value!}")))
        select Flow.Continue;

    private static Flow<Flow> ExecutionFlow(ExecutionDeposition execution) =>
        from style in Pulse.Draw<Styles>()
        from _0 in newLine
        from _2 in Pulse.ToFlowIf(style == Styles.Default, InputFlow, () => execution.InputDepositions)
        from _3 in Pulse.ToFlowIf(style == Styles.Oracle, OracleInputFlow,
            () => new OracleInput(execution.InputDepositions.Single(), execution.TraceDepositions, execution.FinalTraceDepositions))
        from _4 in Pulse.ToFlow(WarningFlow, execution.GetWarningDepositionsForReport())
        from _5 in newLine.Then(space).Then(LineOf(60))
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

    private static Flow<Flow> StyleGuide(CaseFile caseFile) =>
        from _1 in Pulse.ToFlowIf(caseFile.HasEvidence, EvidenceFlow, () => caseFile)
        from _2 in Pulse.OnType((CaseSummary a) => SummaryFlow(a), () => caseFile)
        select Flow.Continue;

    /// <summary>
    /// Formats a case file using the default QuickTestr report style.
    /// Use for standard property-based Testr output.
    /// </summary>
    public static Flow<Flow> DefaultStyleGuide(CaseFile caseFile) =>
        Pulse.Prime(() => Styles.Default)
            .Then(StyleGuide(caseFile));

    /// <summary>
    /// Formats a case file using the oracle QuickTestr report style.
    /// Use for expected-versus-actual Testr output.
    /// </summary>
    public static Flow<Flow> OracleStyleGuide(CaseFile caseFile) =>
        Pulse.Prime(() => Styles.Oracle)
            .Then(StyleGuide(caseFile));
}
