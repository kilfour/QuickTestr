namespace QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model.AstNodes;

public record UnaryMinus(AstNode Value) : AstNode
{
    public override double Eval() => 0 - Value.Eval();
}

