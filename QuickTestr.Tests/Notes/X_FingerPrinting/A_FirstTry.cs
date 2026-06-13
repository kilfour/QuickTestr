using QuickCheckr;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.FilingCabinet;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Notes.X_FingerPrinting;


[DocFile]
public class A_FirstTry : TestrPropertyTest<A_FirstTry>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact(Skip = "Touches the FileSystem, use InMemoryCustodian")]
    [DocTestrHeader]
    [DocTestr]
    [DocBoldHeader("The Runner")]
    [DocExample(typeof(A_FirstTry), nameof(RunIt))]
    [DocReportHeader]
    [DocReport]
    public override void Example() => Run(RunIt);

    [CodeSnippet]
    private ITestrRunner RunIt() =>
        GetTestr()
            .WithVault<int>()
            .FillVault(5.Searches(), 10.Runs());

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr.Named("Not between [40, 50]")
            .DisableValueReduction()
            .For(Fuzzr.Int())
            .Assert(a => a > 50 || a < 40);

    protected override void Verify(Article article)
    {

    }
}