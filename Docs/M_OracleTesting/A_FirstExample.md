# First Example

**The Testr:**  
```csharp
Testr.Named("AddBuggy Matches AddCorrect")
    .For(Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int()))
    .Expected(a => AddCorrect(a.Item1, a.Item2))
    .Actual(a => AddBuggy(a.Item1, a.Item2));
```

**Expected:**  
```csharp
public int AddCorrect(int a, int b) => a + b;
```

**Actual:**  
```csharp
public int AddBuggy(int a, int b) => a > 42 ? 0 : a + b;
```

**The Report:**  
```text
------------------------------------------------------------
  AddBuggy Matches AddCorrect
  Seed: 1471595869
 ------------------------------------------------------------
  Falsified:
    Input    = ( 94, _ )
    Redux    = ( 43, _ )

    Expected = 129
    Actual   = 0

  Original:
    ( 94, 35 )
 ------------------------------------------------------------
```
