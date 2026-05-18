using QuickCheckr;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickCheckr.UnderTheHood.Proceedings.ClerksOffice;
using QuickCheckr.UnderTheHood.Proceedings.Depositions;
using QuickPulse;

namespace QuickTestr.Bolts;



/// <summary>
/// Provides the built-in report style guides used by QuickTestr.
/// Use when you want the standard compact formatting for property-based cases.
/// </summary>
public static class PropertyStyleGuide
{
    /// <summary>
    /// Formats a case file using the property-based QuickTestr report style.
    /// Use for standard property-based Testr output.
    /// </summary>
    public static Flow<Flow> Render(CaseFile caseFile) =>
        CommonStyleGuide.Render(caseFile, ExecutionFlow);

    private static Flow<Flow> ExecutionFlow(ExecutionDeposition execution) =>
        Pulse
            .ToFlow(InputFlow, execution.InputDepositions)
            .ToFlow(CommonStyleGuide.WarningsFlow, execution.GetWarningDepositionsForReport());

    private static Flow<Flow> InputFlow(InputDeposition input) =>
        Style
            .OnNewLine().Indent(3).LabeledValue("Input", input.Value)
            .When(input.Redux.HasValue,
                Style
                    .OnNewLine()
                    .Indent(3).LabeledValue("Redux", input.Redux.Value!))
            .When(input.Original.HasValue && !Equals(input.Value, input.Original.Value),
                Style
                    .NewLine()
                    .OnNewLine().Indent(1).Caption("Original"))
                    .OnNewLine().Indent(3).Trace(input.Original.Value!);
}
