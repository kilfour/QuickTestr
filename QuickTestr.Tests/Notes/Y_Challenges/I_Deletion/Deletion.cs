using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickFuzzr;
using QuickPulse.Explains;

namespace QuickTestr.Tests.Notes.Y_Challenges.I_Deletion;

[DocFile]
[DocContent(
@"
This tests the property ""if we remove an element from a list, the element is
no longer in the list"". The remove function we use however only actually
removes the _first_ instance of the element, so this fails whenever the list
contains a duplicate and we try to remove one of those elements.

This example is interesting for a couple of reasons:

1. It's a nice easy to explain example of property-based testing.
2. Shrinking duplicates simultaneously is something that most property-based
   testing libraries can't do.

The expected smallest falsified sample is `([0, 0], 0)`.
")]
public class Deletion : TestrPropertyTest<Deletion>
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
        Run(712389878);

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr
            .Named("Element is no longer in the list.")
            .For(Fuzzr.Tuple(Fuzzr.Int(1, 10).Many(3, 20), Fuzzr.Int(1, 10)))
            .Assert(a =>
            {
                var list = a.Item1.ToList();
                list.Remove(a.Item2);
                return !list.Contains(a.Item2);
            });

    protected override void Verify(Article article)
    {
        Assert.Equal("Element is no longer in the list.", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(0, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("( [ 1, 1 ], 1 )", article.Execution(1).Input(1).Read().Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
    }
}
