using QuickCheckr.FilingCabinet;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickPulse.Explains;
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
[DocExample(typeof(D_ButICareAboutTheException), nameof(Example))]
[DocReportHeader]
[DocReport]
public class D_ButICareAboutTheException : TestrModelRunTest<D_ButICareAboutTheException>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public void RunExample() => Run(Example, a => { });

    [CodeSnippet]
    private static ConfiguredCheckr Example() =>
        Testr.Named("NameCollector matches model")
            .Model(() => new NameCollectorModel())
            .Sut(() => new NameCollector())
            .VerifyReturnValues()
            .Operation("Add", Fuzzr.String(1),
                (model, a) => model.Add(a),
                (sut, a) => sut.Add(a))
            .Observe("Result Matches",
                (model, sut) => model.Names.SequenceEqual(sut.Names), a => a.Trace())
            .Run();
}

