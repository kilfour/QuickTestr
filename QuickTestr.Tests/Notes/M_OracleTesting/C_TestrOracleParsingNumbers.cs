using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickPulse.Explains;
using QuickCheckr;
using QuickTestr.Tests.Notes.K_Examples.N_ParsingNumber.Model;
using QuickTestr.Tests.Notes.K_Examples.N_ParsingNumber;
using QuickCheckr.StringReduction;

namespace QuickTestr.Tests.Notes.M_OracleTesting;

[DocFile]
public class C_TestrOracleParsingNumbers : TestrPropertyTest<C_TestrOracleParsingNumbers>
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
        Run(1026389787);

    private readonly char[] operators = ['+', '-', '*', '/', '^'];

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr
            .Named("Interpreter matches golden model.")
            .For(ExpressionFuzzr.GetExpression(), ExpressionReducer())
            .DisableValueReduction()
            .Deliberate(a => a.Length, 4)
            .Expected(a => LostIn.Translation(a).Eval())
            .Actual(a => LostIn.FaultyTranslation(a).Eval());

    private Shrinker ExpressionReducer() =>
        Shrink.OnType<string>(s => s.Simplify(
            Select.While(char.IsWhiteSpace).Remove(),
            Select.Balanced('(', ')').Delimiters().Remove(),
            Select.While(char.IsDigit).OneOf(operators).Remove(),
            Select.OneOf(operators).While(char.IsDigit).Remove(),
            Select.OneOf('-').OneOf('-').While(a => a == '-').Replace("-")));

    protected override void Verify(Article article)
    {
        Assert.Equal("Interpreter matches golden model.", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(2, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(2, article.Total().Traces());
        Assert.Equal(1, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Actual", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Expected", article.Execution(1).Action(2).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("\"2 ^ (1 / 3 - 2 * 1 / 1 + 3 + 2 / 1 / 3) / - 2 ^ 3 * - 3 ^ 2\"", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("\"2 ^ (1 / 3 - 2 * 1 / 1 + 3 + 2 / 1 / 3) / - 2 ^ 3 * - 3 ^ 2\"", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("\"-3^2\"", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
        Assert.Equal("Expected", article.Execution(1).Trace(1).Read().Label);
        Assert.Equal("4.5", article.Execution(1).Trace(1).Read().Value);
        Assert.False(article.Execution(1).Trace(1).Read().Labeled);
        Assert.Equal("Actual  ", article.Execution(1).Trace(2).Read().Label);
        Assert.Equal("-4.5", article.Execution(1).Trace(2).Read().Value);
        Assert.False(article.Execution(1).Trace(2).Read().Labeled);
    }
}

