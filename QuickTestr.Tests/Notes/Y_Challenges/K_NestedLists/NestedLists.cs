using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickFuzzr;
using QuickPulse.Explains;

namespace QuickTestr.Tests.Notes.Y_Challenges.K_NestedLists;

[DocFile]
public class NestedLists : TestrPropertyTest<NestedLists>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    [DocTestrHeader]
    [DocTestr]
    [DocReportHeader]
    [DocReport]
    public override void Example() =>
        Run(1959968277);

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr
            .Named("The sum of lengths of the element lists is at most 10.")
            .For(Fuzzr.Int().Many(0, 20).ToList().Many(0, 20).ToList())
            .Deliberate(a => a.Count)
            .Assert(a => a.Sum(a => a.Count) <= 10);

    protected override void Verify(Article article)
    {
        Assert.Equal("The sum of lengths of the element lists is at most 10.", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(31, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("[ [ _, _, _, _, _, _, _, _, _, _, _ ] ]", article.Execution(1).Input(1).Read().Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
    }
}