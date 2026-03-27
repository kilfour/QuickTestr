# Testr Parsing Numbers

**The Testr:**  
```csharp
Testr
    .Named("Parser matches golden model.")
    .For(ParseFuzzr.Expression(),
        Shrink.OnType<string>(
            s => s.Simplify(Reduce.Function<string>(ParseReducer.Reducer, true))))
    .DisableValueReduction()
    .Deliberate(a => a.Length, 4)
    .Assert(a => LostIn.Translation(a).Eval() == LostIn.FaultyTranslation(a).Eval());
```
`.Run()` is called without arguments, so it performs the default 100.Runs().  

**The Report:**  
```text
------------------------------------------------------------
  Parser matches golden model.
  Seed: 43650243
 ------------------------------------------------------------
  Falsified:
    Input = "- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)"
    Redux = "-2^2"

  Original:
    "- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)"
 ------------------------------------------------------------
```

**The Fuzzr:**  
```csharp
public class ParseFuzzr
{
    public static FuzzrOf<string> Expression(int maxDepth = 4) => Expr(maxDepth);
    public static readonly FuzzrOf<string> Number = from i in Fuzzr.Int(1, 4) select i.ToString();
    private static readonly FuzzrOf<string> AddOp = Fuzzr.OneOf("+", "-");
    private static readonly FuzzrOf<string> MulOp = Fuzzr.OneOf("*", "/");
    private static readonly FuzzrOf<string> PowOp = Fuzzr.Constant("^");
    private static readonly FuzzrOf<string> WS =
        from chs in Fuzzr.Constant(' ').Many(1)
        select new string([.. chs]);
    private static readonly FuzzrOf<string> UnaryOp = Fuzzr.OneOf("-");
    private static FuzzrOf<string> Primary(int maxDepth) =>
        maxDepth <= 0
        ? Number
        : Fuzzr.OneOf(
            Number,
            from l in Fuzzr.Constant("(")
            from e in Expr(maxDepth - 1)
            from r in Fuzzr.Constant(")")
            select l + e + r,
            from op in UnaryOp
            from ws in WS
            from inner in Primary(maxDepth - 1)
            select op + ws + inner
          );
    private static FuzzrOf<string> Factor(int maxDepth) =>
        from a in Primary(maxDepth)
        from tail in maxDepth <= 0
            ? Fuzzr.Constant("")
            : Fuzzr.OneOf(
                  Fuzzr.Constant(""),
                  from w1 in WS
                  from op in PowOp
                  from w2 in WS
                  from b in Factor(maxDepth - 1)
                  select w1 + op + w2 + b)
        select a + tail;
    private static FuzzrOf<string> Term(int maxDepth) =>
        from head in Factor(maxDepth)
        from pieces in (
            from w1 in WS
            from op in MulOp
            from w2 in WS
            from rhs in Factor(maxDepth - 1)
            select w1 + op + w2 + rhs
        ).Many(0, 3)
        select head + string.Concat(pieces);
    private static FuzzrOf<string> Expr(int maxDepth) =>
        from depth in Fuzzr.Int(0, maxDepth)
        from head in Term(depth)
        from pieces in (
            from w1 in WS
            from op in AddOp
            from w2 in WS
            from rhs in Term(depth - 1)
            select w1 + op + w2 + rhs
        ).Many(0, 3)
        select head + string.Concat(pieces);
}
```

**The Reducer:**  
```csharp
public static class ParseReducer
{
    public static IEnumerable<string> Reducer(string expr)
    {
        foreach (var item in RemoveOperations(expr))
            yield return item;
        string output = Regex.Replace(expr, @"\([^()]*\)", "2");
        yield return output;
        foreach (var item in RemoveOperations(output))
            yield return item;
    }
    private static IEnumerable<string> RemoveOperations(string expr)
    {
        var tokens = Lexer.Tokenize(expr);
        tokens = tokens.Where(a => a.Kind != TokenKind.LParen && a.Kind != TokenKind.RParen);
        yield return TokensToString(tokens);
        var list = tokens.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            yield return TokensToString(tokens.Skip(i));
            if (list[i].IsOperator && !list[i + 1].IsOperator)
                yield return TokensToString(tokens.Skip(i + 1));
        }
        for (int i = list.Count - 1; i > 0; i--)
        {
            if (list[i].IsOperator && !list[i - 1].IsOperator)
                yield return TokensToString(tokens.Take(i));
        }
    }
    private static string TokensToString(IEnumerable<Token> tokens)
        => string.Join("", tokens.Select(a => a.Text));
}
```
