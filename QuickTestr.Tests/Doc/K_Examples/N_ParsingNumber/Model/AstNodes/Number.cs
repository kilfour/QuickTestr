namespace QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model.AstNodes;

public record Number(double Value) : AstNode
{
    public override double Eval() => Value;
}

