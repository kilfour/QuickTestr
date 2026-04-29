using QuickTestr.Tests.Tools;
using QuickPulse.Explains;
using QuickCheckr;
using QuickCheckr.StringReduction;
using QuickTestr.Tests.Docs.B_OracleBased.Sub.Model;
using QuickCheckr.Authoring.ThePress.Printing;

namespace QuickTestr.Tests.Docs.B_OracleBased.Sub;

[DocFile]
public class B_AnInterpreter : TestrOracleTest<B_AnInterpreter>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    [DocTestrHeader]
    [DocTestr]
    [DocReportHeader]
    [DocReport]
    [DocBoldHeader("Domain Aware String Reduction Rules")]
    [DocExample(typeof(Remove))]
    public override void Example() =>
        Run(443608219);

    [CodeSnippet]
    protected override ITestrRunner GetTestr() =>
        Testr
            .Named("Interpreter matches golden model.")
            .For(ExpressionFuzzr.GetExpression(),
                Shrink.OnType<string>(
                    s => s.Simplify(
                        Remove.WhiteSpace,
                        Remove.BalancedParentheses,
                        Remove.LeftHandSideAndOperator,
                        Remove.UnaryMinusNoise)))
            .Deliberate(a => a.Length, 4) // prefer smaller length strings
            .Expected(a => LostIn.Translation(a).Eval())
            .Actual(a => LostIn.FaultyTranslation(a).Eval());

    [CodeExample]
    public class Remove
    {
        public static readonly SelectrOf<Selection> WhiteSpace =
            Stringr.AtleastOnce(char.IsWhiteSpace).Remove().Defined();

        public static readonly SelectrOf<Selection> BalancedParentheses =
            Stringr.Balanced('(', ')').Replace(a => a[1..^1]).Defined();

        public static readonly SelectrOf<Selection> LeftHandSideAndOperator =
            from lhs in Stringr.AtleastOnce(char.IsDigit).Remove()
            from op in Stringr.Once('+', '-', '*', '/', '^').Remove()
            select Selection.Defined;

        public static readonly SelectrOf<Selection> UnaryMinusNoise =
            Stringr.AtleastOnce('-').Replace(_ => "-").Defined();
    }

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
        Assert.Equal("\"1 ^ 1 ^ - 3 ^ 1 / - 1 ^ (2 / 2 + 1 - 3 / 3 / 3) * 3 - 1 ^ (2 - 2 / 1 + 2 / 1 / 3) + 3 / (1 + 3 + 2 * 3) / 2 * - 2 ^ 2\"", article.Execution(1).Input(1).Read().Value);
        Assert.Equal("\"1 ^ 1 ^ - 3 ^ 1 / - 1 ^ (2 / 2 + 1 - 3 / 3 / 3) * 3 - 1 ^ (2 - 2 / 1 + 2 / 1 / 3) + 3 / (1 + 3 + 2 * 3) / 2 * - 2 ^ 2\"", article.Execution(1).Input(1).Read().Original.Value);
        Assert.Equal("\"-2^2\"", article.Execution(1).Input(1).Read().Redux.Value);
        Assert.False(article.Execution(1).Input(1).Read().Labeled);
        Assert.Equal("Expected", article.Execution(1).Trace(1).Read().Label);
        Assert.Equal("-4.6", article.Execution(1).Trace(1).Read().Value);
        Assert.False(article.Execution(1).Trace(1).Read().Labeled);
        Assert.Equal("Actual  ", article.Execution(1).Trace(2).Read().Label);
        Assert.Equal("NaN", article.Execution(1).Trace(2).Read().Value);
        Assert.False(article.Execution(1).Trace(2).Read().Labeled);

    }
}

