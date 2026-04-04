using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickPulse.Explains;
using QuickCheckr;
using QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model;
using QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber;

namespace QuickTestr.Tests.Doc.M_OracleTesting;

[DocFile]
public class C_TestrOracleParsingNumbers : TestrTest<C_TestrOracleParsingNumbers>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    [DocContent("This test reuses the model, sut, fuzzr and reducer from the 'Testr Parsing Numbers' example.")]
    [DocTestrHeader]
    [DocTestr]
    [DocReportHeader]
    [DocReport]
    public override void Example() =>
        Run(43650243);

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr
            .Named("Parser matches golden model.")
            .For(ParseFuzzr.Expression(),
                Shrink.OnType<string>(
                    s => s.Simplify(Reduce.Function<string>(ParseReducer.Reducer, true))))
            .DisableValueReduction()
            .Deliberate(a => a.Length, 4)
            .Expected(a => LostIn.Translation(a).Eval())
            .Actual(a => LostIn.FaultyTranslation(a).Eval());

    protected override void Verify(Article article)
    {
        Assert.Equal("Expected", article.Execution(1).Action(2).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("\"- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)\"", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("\"- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)\"", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("\"-2^2\"", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
        Assert.Equal("Expected", article.Execution(1).Trace(1).Read().Label);
        Assert.Equal("-64.13024747087601", article.Execution(1).Trace(1).Read().Value);
        Assert.False(article.Execution(1).Trace(1).Read().Labeled);
        Assert.Equal("Actual  ", article.Execution(1).Trace(2).Read().Label);
        Assert.Equal("NaN", article.Execution(1).Trace(2).Read().Value);
        Assert.False(article.Execution(1).Trace(2).Read().Labeled);
    }
}
