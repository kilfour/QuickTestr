# Testr Oracle Parsing Numbers
This test reuses the model, sut, fuzzr and reducer from the 'Testr Parsing Numbers' example.  

**The Testr:**  
```csharp
Testr
    .Named("Parser matches golden model.")
    .For(ParseFuzzr.Expression(),
        Shrink.OnType<string>(
            s => s.Simplify(Reduce.Function<string>(ParseReducer.Reducer, true))))
    .DisableValueReduction()
    .Deliberate(a => a.Length, 4)
    .Expected(a => LostIn.Translation(a).Eval())
    .Actual(a => LostIn.FaultyTranslation(a).Eval());
```

**The Report:**  
```text
------------------------------------------------------------
  Parser matches golden model.
  Seed: 43650243
 ------------------------------------------------------------
  Falsified:
    Input = "- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)"
    Redux = "-2^2"

    Expected = -64.13024747087601
    Actual   = NaN

  Original:
    "- (2 - 1 / 3 + 2) ^ (1 / 1 * 3 - 1 / 3 / 1 + 1 * 3 / 2 / 1) / (2 - 1 / 1) / (1 + 1 / 2 * 3 - 2 / 1 * 1 + 1 * 3)"
 ------------------------------------------------------------
```
