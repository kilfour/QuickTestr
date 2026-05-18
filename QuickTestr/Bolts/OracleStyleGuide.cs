using QuickCheckr.FilingCabinet;
using QuickCheckr.FilingCabinet.Depositions;
using QuickCheckr.UnderTheHood.Proceedings.ClerksOffice;
using QuickPulse;

namespace QuickTestr.Bolts;


/// <summary>
/// Provides the built-in report style guides used by QuickTestr.
/// Use when you want the standard compact formatting for oracle-based cases.
/// </summary>
public static class OracleStyleGuide
{
    /// <summary>
    /// Formats a case file using the oracle QuickTestr report style.
    /// Use for expected-versus-actual Testr output.
    /// </summary>
    public static Flow<Flow> Render(IRecord record) =>
        CommonStyleGuide.Render(record, ExecutionFlow);

    private static Flow<Flow> ExecutionFlow(ExecutionDeposition execution) =>
        Pulse
            .ToFlowIf(execution.InputDepositions.Count != 0, OracleInputFlow, () => GetInputAndTraces(execution))
            .ToFlow(CommonStyleGuide.WarningsFlow, execution.GetWarningDepositionsForReport());

    private record OracleInput(InputDeposition Input, List<TraceDeposition> Traces, List<TraceDeposition> FinalTraces);

    private static OracleInput GetInputAndTraces(ExecutionDeposition execution) =>
        new(execution.InputDepositions.Single(), execution.TraceDepositions, execution.FinalTraceDepositions);

    private static Flow<Flow> OracleInputFlow(OracleInput oracleInput) =>
        Style
            .NewLine()
            .OnNewLine()
            .Indent(3)
            .LabeledValue("Input", oracleInput.Input.Value)
            .ToFlow(TracesFlow, oracleInput.Traces)
            .When(oracleInput.Input.Redux.HasValue,
                Style
                    .NewLine()
                    .OnNewLine()
                    .Indent(3)
                    .LabeledValue("Redux", oracleInput.Input.Redux.Value!)
                    .ToFlow(TracesFlow, oracleInput.FinalTraces))
            .When(oracleInput.Input.Original.HasValue && !Equals(oracleInput.Input.Value, oracleInput.Input.Original.Value),
                Style
                    .NewLine()
                    .OnNewLine()
                    .Indent(1)
                    .Caption("Original")
                    .OnNewLine()
                    .Indent(3)
                    .Trace(oracleInput.Input.Original.Value!));

    private static Flow<Flow> TracesFlow(List<TraceDeposition> traces) =>
        Pulse.When(traces.Count > 0,
            Style
                .OnNewLine()
                .Indent(5)
                .Caption("Observed"))
                .ToFlow(TraceFlow, traces);

    private static Flow<Flow> TraceFlow(TraceDeposition trace)
        => Style
            .OnNewLine()
            .Indent(7)
            .LabeledValue(trace.Label, trace.Value);
}
