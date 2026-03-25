namespace QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model.AstNodes;

public record Multiplication(AstNode Left, AstNode Right) : AstNode
{
    public override double Eval() => Left.Eval() * Right.Eval();
}

