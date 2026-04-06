# Property-based Style Testing
In property-based testing you describe **what should always be true**, regardless of the input.  
QuickTestr generates many inputs and tries to falsify your rule, shrinking failures to a minimal example.

I guess by now it's obligatory so let's go and start by reversing a string:  
```csharp
Testr.Named("Reverse is its own inverse")              // The name of the property.
    .For(Fuzzr.Int().Many(1, 10))                      // The input Fuzzr.
    .Assert(s => Reverse(Reverse(s)).SequenceEqual(s)) // The property to assert.
    .Run();                                            // Run the test.
// --------------------------------------------------------------------------------
static IEnumerable<int> Reverse(IEnumerable<int> l) => [.. l.Reverse()];
```
That one passes of course, which is not really interesting.  
So let's throw some spanners in the works by moving all numbers `42` to the end.  
```csharp
Testr.Named("Reverse is its own inverse")
    .For(Fuzzr.Int().Many(1, 10))
    .Assert(s => Reverse(Reverse(s)).SequenceEqual(s))
    .Run();
// ---------------------------------------------------------------------
static IEnumerable<int> Reverse(IEnumerable<int> l) => [.. HideTheAnswer(l.Reverse())];
static IEnumerable<int> HideTheAnswer(IEnumerable<int> l)
    => l.Where(a => a != 42).Concat(l.Where(a => a == 42));
```

**The Report:**  
```text
------------------------------------------------------------
  Reverse is its own inverse
  Seed: 174616483
 ------------------------------------------------------------
  Falsified:
    Input = [ 42, _ ]

  Original:
    [ 86, 33, 42, 21, 7, 62, 44, 10 ]
 ------------------------------------------------------------
```
