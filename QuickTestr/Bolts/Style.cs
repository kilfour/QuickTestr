using QuickPulse;

namespace QuickTestr.Bolts;


public static class Style
{
    public static string Pluralize(int count, string str) =>
        count > 1 ? $"{str}s" : str;

    public static Flow<Flow> Space() => Pulse.Trace(" ");
    public static Flow<Flow> Space(this Flow<Flow> other)
        => other.Then(Space());

    public static Flow<Flow> Indent(int level)
        => Pulse.Trace(new string(' ', level));
    public static Flow<Flow> Indent(this Flow<Flow> other, int level)
        => other.Then(Indent(level));

    public static Flow<Flow> NewLine()
        => Pulse.Trace(Environment.NewLine);
    public static Flow<Flow> NewLine(this Flow<Flow> other)
        => other.Then(NewLine());

    public static Flow<Flow> OnNewLine()
        => NewLine().Then(Space());
    public static Flow<Flow> OnNewLine(this Flow<Flow> other)
        => other.Then(OnNewLine());

    private static Flow<Flow> LineOf(int length) => Pulse.Trace(new string('-', length));
    public static Flow<Flow> DrawTopLine() =>
        Space().Then(LineOf(60));
    public static Flow<Flow> DrawLine() =>
        OnNewLine().Then(LineOf(60));
    public static Flow<Flow> DrawLine(this Flow<Flow> other) =>
        other.Then(DrawLine());

    public static Flow<Flow> LabeledValue(string label, object value) =>
        Pulse.Trace($"{label} = {value}");
    public static Flow<Flow> LabeledValue(this Flow<Flow> other, string label, object value) =>
        other.Then(LabeledValue(label, value));

    public static Flow<Flow> Caption(string label) =>
        Pulse.Trace($"{label}:");
    public static Flow<Flow> Caption(this Flow<Flow> other, string label) =>
        other.Then(Caption(label));
}

