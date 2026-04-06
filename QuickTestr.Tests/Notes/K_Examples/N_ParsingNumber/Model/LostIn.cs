using QuickTestr.Tests.Notes.K_Examples.N_ParsingNumber.Model.AstNodes;

namespace QuickTestr.Tests.Notes.K_Examples.N_ParsingNumber.Model;

public class LostIn
{
    public static AstNode Translation(string input)
        => new Parser().Parse(Lexer.Tokenize(input));

    public static AstNode FaultyTranslation(string input)
        => new Parser(6).Parse(Lexer.Tokenize(input));
}