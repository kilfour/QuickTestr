using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickPulse.Explains;

namespace QuickTestr.Tests.Notes.Y_Challenges.F_Difference.Sub;

[DocFile]
[DocContent(
@"
Test 1 ('difference must not be zero') only succeeds if 
- the first parameter is less than 10 
- _or_ the difference is not zero.
The smallest falsified sample is `[10, 10]`
")]
public class Test1 : TestrPropertyTest<Test1>
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
        Run(1485535450);

    [CodeSnippet]
    [CodeRemove("Difference.")]
    [CodeRemove("The.")]
    protected override ITestrRunner GetTestr() =>
        Testr.Named("Difference must not be zero.")
            .For(Difference.TheFuzzr, Difference.The.Shrinkr)
            .Assert(a => a.A < 10 || a.A != a.B);

    protected override void Verify(Article article)
    {
        Assert.Equal("Difference must not be zero.", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(1, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("{ A: 93, B: 93 }", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("{ A: 10, B: 10 }", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
    }
}