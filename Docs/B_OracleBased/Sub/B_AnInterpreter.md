# An Interpreter

**The Testr:**  
```csharp
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
    .Expected(a => LostIn.Translation(a).Eval())
    .Actual(a => LostIn.FaultyTranslation(a).Eval());
```

**The Report:**  
```text
------------------------------------------------------------
  Interpreter matches golden model.
  Seed: 443608219
 ------------------------------------------------------------
  Falsified:
    Input    = "1 ^ 1 ^ - 3 ^ 1 / - 1 ^ (2 / 2 + 1 - 3 / 3 / 3) * 3 - 1 ^ (2 - 2 / 1 + 2 / 1 / 3) + 3 / (1 + 3 + 2 * 3) / 2 * - 2 ^ 2"
    Redux    = "-2^2"

  Observed:
    Expected = -4.6
    Actual   = NaN
 ------------------------------------------------------------
```
