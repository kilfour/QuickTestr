using QuickCheckr;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.FilingCabinet;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Notes.X_FingerPrinting;


[DocFile]
public class B_Oracled : TestrOracleTest<B_Oracled>
{
    protected override bool Asserts => false;
    protected override bool Report => true;
    protected override bool Explain => true;

    [Fact]
    [DocTestrHeader]
    [DocTestr]
    [DocBoldHeader("The Runner")]
    [DocExample(typeof(B_Oracled), nameof(RunIt))]
    [DocReportHeader]
    [DocReport]
    public override void Example() => Run(RunIt);

    [CodeSnippet]
    private IRecord RunIt() =>
        GetTestr()
            .WithVault<int>()
            .FillVault(200.Searches(), 200.Runs());

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr.Named("Oracle Not between [40, 50]")
            //.DisableValueReduction()
            .For(Fuzzr.Int())
            .Expected(a => a)
            .Actual(a => (a > 50 || a < 40) ? a : 0);

    protected override void Verify(Article article)
    {

    }
}