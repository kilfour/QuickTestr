# What If It Throws

**The Testr:**  
```csharp
Testr.Named("AddBuggy Matches AddCorrect")
    .For(Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int()))
    .Expected(a => AddCorrect(a.Item1, a.Item2))
    .Actual(a => AddBuggy(a.Item1, a.Item2));
```

**Expected:**  
```csharp
public int AddCorrect(int a, int b)
{
    if (a < 10)
        ComputerSays.No<int>("a is too small");
    return a + b;
}
```

**Actual:**  
```csharp
public int AddBuggy(int a, int b)
{
    if (a <= 10)
        ComputerSays.No<int>("a is too small");
    return a + b;
}
```

**The Report:**  
```text
------------------------------------------------------------
  AddBuggy Matches AddCorrect
  Seed: 1055521326
 ------------------------------------------------------------
  Falsified:
    Input    = ( 10, _ )

    Expected = 23
    Actual   = ComputerSaysNo: a is too small

  Original:
    ( 10, 13 )
 ------------------------------------------------------------
```
