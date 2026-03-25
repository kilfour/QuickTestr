namespace QuickTestr.Tests.Doc.K_Examples.N_ParsingNumber.Model.Lexing;

public record Token(TokenKind Kind, string Text, int Offset)
{
    private static readonly TokenKind[] operators =
        [ TokenKind.Plus
        , TokenKind.Minus
        , TokenKind.Star
        , TokenKind.Slash
        , TokenKind.Caret
        ];

    public bool IsOperator => operators.Contains(Kind);
};