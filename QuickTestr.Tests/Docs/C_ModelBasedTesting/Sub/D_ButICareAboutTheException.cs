using QuickCheckr;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
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
[DocTestr]
[DocReportHeader]
[DocReport]
public class D_ButICareAboutTheException : QuickTestrModelRunTest<D_ButICareAboutTheException>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public override void Example() => Document();

    [CodeSnippet]
    [CodeRemoveJournalist]
    protected override void GetTestr(Journalist journalist) =>
        Testr.Named("NameCollector matches model")
            .Model(() => new NameCollectorModel())
            .Sut(() => new NameCollector())
            .VerifyOperationResults()
            .Operation("Add", Fuzzr.String(1),
                (model, a) => model.Add(a),
                (sut, a) => sut.Add(a))
            .Observe("Result Matches",
                (model, sut) => model.Names.SequenceEqual(sut.Names), a => a.Trace())
            .StoreCaseFiles(journalist)
            .Run();

    protected override void Verify(Article article)
    {
    }
}

