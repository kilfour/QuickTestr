# QuickTestr
QuickTestr currently supports two styles:
- [Property-based][PropertyBased] : Define what should always hold.
- [Oracle-based][OracleBased]: Compare against something that already works.  

[PropertyBased]: #property-based-style-testing

[OracleBased]: #oracle-based-style-testing
## Property-based Style Testing
In property-based testing you describe **what should always be true**, regardless of the input.  
QuickTestr generates many inputs and tries to falsify your rule, shrinking failures to a minimal example.

A classic to begin with:  
### Reversing a List
```csharp
Testr.Named("Reverse is its own inverse")              // The name of the property.
    .For(Fuzzr.Int().Many(1, 10))                      // The input Fuzzr.
    .Assert(s => Reverse(Reverse(s)).SequenceEqual(s)) // The property to assert.
    .Run();                                            // Run the test.
// --------------------------------------------------------------------------------
static IEnumerable<int> Reverse(IEnumerable<int> l) => [.. l.Reverse()];
```
That passes, which isn't very interesting.
Let's break it by moving all `42`s to the end.  
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
### Moving On
Now for something less trivial, one of the `jqwik` challenges.  

The property we are testing states:  
> No two different elements point to each other when used as indexes into the list.  

**The Testr:**  
```csharp
Testr
    .Named("No two different indexes point to each other.")
    .For(
        from length in Fuzzr.Int()
        from enumerable in Fuzzr.Int(0, length - 1).Many(length)
        select enumerable.ToList())
    .DisableValueReduction()
    .Deliberate(a => a.Count, 2)
    .Assert(list => list.All(
        element =>
        {
            int index = list[element];
            if (index != element && list[index] == element)
                return false;
            return true;
        }));
```
**Some Notes:**
- `DisableValueReduction()`: By default QuickTestr tries to move ints towards zero, but here that isn't necessary.
- `Deliberate(a => a.Count, 2)`: This is a *Deliberation Policy*. It helps escape local minima during shrinking. In this case, we're looking for smaller lists.
- The compositional abilities of QuickFuzzr allow us to generate only valid indexes, keeping inputs structurally sound.  

**The Report:**  
```text
------------------------------------------------------------
  No two different indexes point to each other.
  Seed: 1934478623
 ------------------------------------------------------------
  Falsified:
    Input = [ 1, 0 ]

  Original:
    [ 1, 1, 0, 4, 3, 3, 2, 3, 11, 6, 4, 4, 4, 12 ]
 ------------------------------------------------------------
```
## Oracle-based Style Testing
The second style of QuickTestr verification.

Still stateless, still one invariant, but now we compare against something that already works: an oracle.

This is a very viable option in many cases. Refactoring legacy code for example.
Or in my case, verifying student code against a golden model.  
### First Example
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
### An Interpreter

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
