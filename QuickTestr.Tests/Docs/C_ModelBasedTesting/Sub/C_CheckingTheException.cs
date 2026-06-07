using QuickCheckr.FilingCabinet;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickPulse.Instruments;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.C_ModelBasedTesting.Sub;

[DocFile]
[DocContent(
"""
Operation exceptions do not fail the model test by themselves.  
They only matter if they lead to an observed state mismatch.
"""
)]
[DocModelHeader]
[DocExample(typeof(NameCollectorModel))]
[DocBoldHeader("SUT")]
[DocExample(typeof(NameCollector))]
[DocTestrHeader]
[DocExample(typeof(C_CheckingTheException), nameof(Example))]
[DocReportHeader]
[DocReport]
public class C_CheckingTheException : TestrRunTest<C_CheckingTheException>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public void RunExample() => Run(Example, a => { });

    [CodeSnippet]
    private static IRecord Example() =>
        Testr.Named("NameCollector matches model")
            .Model(() => new NameCollectorModel())
            .Sut(() => new NameCollector())
            .Operation("Add", Fuzzr.String(1),
                (model, a) => model.Add(a),
                (sut, a) => sut.Add(a))
            .Observe("Result Matches",
                (model, sut) => model.Names.SequenceEqual(sut.Names), a => a.Trace())
            .Run();
}


[CodeExample]
public class NameCollectorModel
{
    private readonly List<string> names = [];
    public IReadOnlyList<string> Names => names;
    public void Add(string name)
    {
        if (!names.Contains(name))
            names.Add(name);
    }
}

[CodeExample]
public class NameCollector
{
    private readonly List<string> names = [];
    public IReadOnlyList<string> Names => names;
    public void Add(string name)
    {
        if (names.Contains(name))
            ComputerSays.No("Already have that one ...");
        names.Add(name);
    }
}