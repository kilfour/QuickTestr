using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickPulse;
using QuickCheckr.UnderTheHood.Proceedings;

namespace QuickTestr.Tests.Doc.M_OracleTesting;

[DocFile]
public class A_FirstExample : TestrTest<A_FirstExample>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    protected override Func<CaseFile, Flow<Flow>> StyleGuide => TheTestr.OracleStyleGuide;

    [Fact]
    [DocTestrHeader]
    [DocTestr]
    [DocBoldHeader("Expected")]
    [DocExample(typeof(A_FirstExample), nameof(AddCorrect))]
    [DocBoldHeader("Actual")]
    [DocExample(typeof(A_FirstExample), nameof(AddBuggy))]
    [DocReportHeader]
    [DocReport]
    public override void Example() =>
        Run(1471595869);

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr.Named("AddBuggy Matches AddCorrect")
            .For(Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int()))
            .Expected(a => AddCorrect(a.Item1, a.Item2))
            .Actual(a => AddBuggy(a.Item1, a.Item2));

    protected override void Verify(Article article)
    {
        Assert.Equal("AddBuggy Matches AddCorrect", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(2, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(2, article.Total().Traces());
        Assert.Equal(1, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Actual", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Expected", article.Execution(1).Action(2).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("( 94, _ )", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("( 94, 35 )", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("( 43, _ )", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
        Assert.Equal("Expected", article.Execution(1).Trace(1).Read().Label);
        Assert.Equal("129", article.Execution(1).Trace(1).Read().Value);
        Assert.False(article.Execution(1).Trace(1).Read().Labeled);
        Assert.Equal("Actual  ", article.Execution(1).Trace(2).Read().Label);
        Assert.Equal("0", article.Execution(1).Trace(2).Read().Value);
        Assert.False(article.Execution(1).Trace(2).Read().Labeled);
    }


    [CodeExample]
    public int AddCorrect(int a, int b) => a + b;

    [CodeExample]
    public int AddBuggy(int a, int b) => a > 42 ? 0 : a + b;
}
