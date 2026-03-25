using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickPulse.Explains;
using QuickCheckr;
using QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model;

namespace QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber;

[DocFile]
public class TestrParsingNumbers : TestrTest<TestrParsingNumbers>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    [DocTestrHeader]
    [DocTestr]
    [DocContent("`.Run()` is called without arguments, so it performs the default 100.Runs().")]
    [DocReportHeader]
    [DocReport]
    [DocBoldHeader("The Fuzzr")]
    [DocExample(typeof(ParseFuzzr))]
    [DocBoldHeader("The Reducer")]
    [DocExample(typeof(ParseReducer))]
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
            .Assert(a => LostIn.Translation(a).Eval() == LostIn.FaultyTranslation(a).Eval());

    protected override void Verify(Article article)
    {
        Assert.Equal("Parser matches golden model.", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(1, article.Total().Traces());
        Assert.Equal(1, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("\"- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)\"", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("\"- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)\"", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("\"-2^2\"", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
        Assert.Equal("Original", article.Execution(1).Trace(1).Read().Label);
        Assert.Equal("\"- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)\"", article.Execution(1).Trace(1).Read().Value);
        Assert.False(article.Execution(1).Trace(1).Read().Labeled);
    }
}
