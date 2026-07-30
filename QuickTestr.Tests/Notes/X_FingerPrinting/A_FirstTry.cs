using QuickCheckr;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Notes.X_FingerPrinting;


[DocFile]
public class A_FirstTry : QuickTestrPropertyTest<A_FirstTry>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    [DocTestrHeader]
    [DocTestr]
    [DocBoldHeader("The Runner")]
    [DocExample(typeof(A_FirstTry), nameof(RunIt))]
    [DocReportHeader]
    [DocReport]
    public override void Example() => Document();

    [CodeSnippet]
    private ITestrRunner RunIt(ITestrRunner testr) =>
        testr
            .WithVault<int>()
            .FillVault(5.Searches(), 10.Runs());

    [CodeSnippet]
    [CodeRemoveJournalist]
    protected override void GetTestr(Journalist journalist)
    {
        var testr = Testr.Named("Not between [40, 50]")
            .DisableValueReduction()
            .For(Fuzzr.Int())
            .Assert(a => a > 50 || a < 40)
            .StoreCaseFiles(journalist);

        RunIt(testr);
    }

    protected override void Verify(Article article)
    {

    }
}
