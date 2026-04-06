# First Example
Starting out with a toy example once again, assume we have the following working method:  
```csharp
public int AddCorrect(int a, int b) => a + b;
```
And we want to compare it against a newer implementation:  
```csharp
public int AddBuggy(int a, int b) => a > 42 ? 0 : a + b;
```
This implementation silently breaks when `a > 42`.  

**The Testr:**  
```csharp
Testr.Named("AddBuggy Matches AddCorrect")
    .For(Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int()))
    .Expected(a => AddCorrect(a.Item1, a.Item2))
    .Actual(a => AddBuggy(a.Item1, a.Item2));
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

  Observed:
    Expected = 129
    Actual   = 0

  Original:
    ( 94, 35 )
 ------------------------------------------------------------
```
