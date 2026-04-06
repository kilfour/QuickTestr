# Testr Oracle Parsing Numbers
This test reuses the model, sut, fuzzr and reducer from the 'Testr Parsing Numbers' example.  

**The Testr:**  
```csharp
Testr
    .Named("Parser matches golden model.")
    .For(ParseFuzzr.Expression(),
        Shrink.OnType<string>(s => s.Simplify([
        Select.While(char.IsWhiteSpace).Remove(),
        Select.Balanced('(', ')').Delimiters().Remove(),
        Select.While(char.IsDigit).OneOf(operators).Remove(),
        Select.OneOf(operators).While(char.IsDigit).Remove(),
        Select.OneOf('-').OneOf('-').While(a => a == '-').Replace("-")
    ])))
    .DisableValueReduction()
    .Deliberate(a => a.Length, 4)
    .Expected(a => LostIn.Translation(a).Eval())
    .Actual(a => LostIn.FaultyTranslation(a).Eval());
```

**The Report:**  
```text
------------------------------------------------------------
  Parser matches golden model.
  Seed: 1026389787
 ------------------------------------------------------------
  Falsified:
    Input = "2 ^ (1 / 3 - 2 * 1 / 1 + 3 + 2 / 1 / 3) / - 2 ^ 3 * - 3 ^ 2"
    Redux = "-3^2"

    Expected = 4.5
    Actual   = -4.5

  Original:
    "2 ^ (1 / 3 - 2 * 1 / 1 + 3 + 2 / 1 / 3) / - 2 ^ 3 * - 3 ^ 2"
 ------------------------------------------------------------
```
