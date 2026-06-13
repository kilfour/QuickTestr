using QuickCheckr;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Bolts.Builders.ModelBased;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.C_ModelBasedTesting.Sub;

[DocFile]
[DocContent(
"""
When activating *strict mode* a mismatch in exceptions do fail the model test.
"""
)]
[DocModelHeader]
[DocExample(typeof(NameCollectorModel))]
[DocBoldHeader("SUT")]
[DocExample(typeof(NameCollector))]
[DocTestrHeader]
[DocExample(typeof(D_ButICareAboutTheException), nameof(Example))]
[DocReportHeader]
[DocReport]
public class D_ButICareAboutTheException : TestrModelRunTest<D_ButICareAboutTheException>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public void RunExample() => Document(Example(), a => a.Run(), _ => { });

    [CodeSnippet]
    [CodeRemove("0, 0.ExecutionsPerRun()")]
    private static IModelrRunner Example() =>
        Testr.Named("NameCollector matches model")
            .Model(() => new NameCollectorModel())
            .Sut(() => new NameCollector())
            .VerifyReturnValues()
            .Operation("Add", Fuzzr.String(1),
                (model, a) => model.Add(a),
                (sut, a) => sut.Add(a))
            .Observe("Result Matches",
                (model, sut) => model.Names.SequenceEqual(sut.Names), a => a.Trace())
            .Run(0, 0.ExecutionsPerRun());
}

