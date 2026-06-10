using QuickTestr.Tests.Tools;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickCheckr.Authoring.ThePress.Printing;

namespace QuickTestr.Tests.Notes.F_DealingWithTuples.A_PropertyBased;

[DocFile]
public class A_TupleArity2 : TestrPropertyTest<A_TupleArity2>
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
        Run(() => GetTestr().Run(649859307));

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr.Named("Tuple2 can deconstruct")
            .For(Fuzzr.Int(), Fuzzr.Int())
            .Assert((a, b) => a == b);

    protected override void Verify(Article article)
    {
        Assert.Equal("Tuple2 can deconstruct", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(1, article.Total().Warnings());
        Assert.Equal(1, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("( _, 31 )", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("( 37, 31 )", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("( _, 0 )", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.Equal("No witness for reducer at 'Input.Item2'.", article.Execution(1).Warning(1).Read().Value);
    }
}
