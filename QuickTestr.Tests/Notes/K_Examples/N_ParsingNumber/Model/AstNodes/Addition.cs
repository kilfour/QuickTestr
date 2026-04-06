namespace QuickTestr.Tests.Notes.K_Examples.N_ParsingNumber.Model.AstNodes;

public record Addition(AstNode Left, AstNode Right) : AstNode
{
    public override double Eval() => Left.Eval() + Right.Eval();
}