using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Tools.ThePress.Printing;
using QuickPulse.Explains;
using QuickCheckr;
using QuickCheckr.StringReduction;
using QuickTestr.Tests.Docs.B_OracleBased.Sub.Model;

namespace QuickTestr.Tests.Notes.K_Examples.N_ParsingNumber;

[DocFile]
public class TestrParsingNumbers : TestrPropertyTest<TestrParsingNumbers>
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
    [DocExample(typeof(ExpressionFuzzr))]
    public override void Example() =>
        Run(742123030);

    private readonly char[] operators = ['+', '-', '*', '/', '^'];

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr
            .Named("Interpreter matches golden model.")
            .For(ExpressionFuzzr.GetExpression(),
                Shrink.OnType<string>(
                    s => s.Simplify(
                        // remove whitespace
                        Select.While(char.IsWhiteSpace).Remove(),
                        // remove balanced parentheses
                        Select.Balanced('(', ')').Delimiters().Remove(),
                        // remove operators + lhs number
                        Select.While(char.IsDigit).OneOf(operators).Remove(),
                        // remove unary minus noise
                        Select.OneOf('-').OneOf('-').While(a => a == '-').Replace("-")
                    )))
            .Deliberate(a => a.Length, 4) // prefer smaller length strings
            .Assert(a => LostIn.Translation(a).Eval() == LostIn.FaultyTranslation(a).Eval());

    protected override void Verify(Article article)
    {
        Assert.Equal("Interpreter matches golden model.", article.FailureDescription());
        Assert.Equal(1, article.Total().Executions());
        Assert.Equal(1, article.Total().Actions());
        Assert.Equal(1, article.Total().Inputs());
        Assert.Equal(1, article.ShrinkCount);
        Assert.Equal(1, article.Execution(1).Read().ExecutionId);
        Assert.Equal("Run", article.Execution(1).Action(1).Read().Label);
        Assert.Equal("Input", article.Execution(1).Input(1).Read().Label);
        Assert.Equal("\"(3 / 3 - 1 * 2 * 3 / 3 + 3 * 1 * 2 / 1) ^ 1 ^ (2 * 3 / 1) ^ 1 / - 2 ^ - 2 * - (2 / 1 * 3 / 2 - 2 * 2 - 3 * 3 * 2 + 3 * 1) * - 2 ^ (1 * 3 / 1 / 2 - 2 - 1 * 2 * 1 - 1) + 3 * - 2 ^ 2 / (3 * 1 / 2 - 3 / 3 * 2) ^ 2 * (3 * 3 * 2 / 2 - 2 / 3 / 3 / 2 - 2 - 3 * 2) ^ 2\"", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("\"(3 / 3 - 1 * 2 * 3 / 3 + 3 * 1 * 2 / 1) ^ 1 ^ (2 * 3 / 1) ^ 1 / - 2 ^ - 2 * - (2 / 1 * 3 / 2 - 2 * 2 - 3 * 3 * 2 + 3 * 1) * - 2 ^ (1 * 3 / 1 / 2 - 2 - 1 * 2 * 1 - 1) + 3 * - 2 ^ 2 / (3 * 1 / 2 - 3 / 3 * 2) ^ 2 * (3 * 3 * 2 / 2 - 2 / 3 / 3 / 2 - 2 - 3 * 2) ^ 2\"", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("\"-2^2\"", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
    }
}
