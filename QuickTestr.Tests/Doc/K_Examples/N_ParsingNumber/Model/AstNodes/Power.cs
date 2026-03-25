namespace QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model.AstNodes;

public record Power(AstNode Left, AstNode Right) : AstNode
{
    public override double Eval() => Math.Pow(Left.Eval(), Right.Eval());
}

