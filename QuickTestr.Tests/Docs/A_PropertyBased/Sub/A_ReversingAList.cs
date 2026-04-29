using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.A_PropertyBased.Sub;

[DocFile]
[DocFileHeader("Reversing a List")]
public class A_ReversingAList : TestrPropertyRunTest<A_ReversingAList>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    [DocExample(typeof(A_ReversingAList), nameof(ReversePass))]
    public void RunReversePass() => Run(ReversePass, VerifyReversePass);

    [CodeSnippet]
    [CodeRemove("return")]
    private static CaseFile ReversePass()
    {
        return
        Testr.Named("Reverse is its own inverse")              // The name of the property.
            .For(Fuzzr.Int().Many(1, 10))                      // The input Fuzzr.
            .Assert(s => Reverse(Reverse(s)).SequenceEqual(s)) // The property to assert.
            .Run();                                            // Run the test.
        // --------------------------------------------------------------------------------
        static IEnumerable<int> Reverse(IEnumerable<int> l) => [.. l.Reverse()];
    }

    private void VerifyReversePass(Article article)
    {
        Assert.Equal("", article.FailureDescription());
        Assert.Equal(1, article.Total().PassedExpectations());
        Assert.Equal(0, article.ShrinkCount);
        Assert.Equal("Reverse is its own inverse", article.PassedExpectation(1).Read().Label);
        Assert.Equal(100, article.PassedExpectation(1).Read().TimesPassed);
    }

    [Fact]
    [DocContent("""
That passes, which isn't very interesting.
Let's break it by moving all `42`s to the end.
""")]
    [DocExample(typeof(A_ReversingAList), nameof(ReversePassFail))]
    [DocReportHeader]
    [DocReport]
    public void RunReversePassFail() => Run(ReversePassFail, VerifyReversePassFail);

    [CodeSnippet]
    [CodeRemove("return")]
    [CodeRemove("174616483")]
    private static CaseFile ReversePassFail()
    {
        return
        Testr.Named("Reverse is its own inverse")
            .For(Fuzzr.Int().Many(1, 10))
            .Assert(s => Reverse(Reverse(s)).SequenceEqual(s))
            .Run(174616483);
        // ---------------------------------------------------------------------
        static IEnumerable<int> Reverse(IEnumerable<int> l) => [.. HideTheAnswer(l.Reverse())];
        static IEnumerable<int> HideTheAnswer(IEnumerable<int> l)
            => l.Where(a => a != 42).Concat(l.Where(a => a == 42));
    }

    private void VerifyReversePassFail(Article article)
    {
        Assert.Equal("Reverse is its own inverse", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(7, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("[ 42, _ ]", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("[ 86, 33, 42, 21, 7, 62, 44, 10 ]", article.Execution(1).Input(1).Read().Original.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
    }
}