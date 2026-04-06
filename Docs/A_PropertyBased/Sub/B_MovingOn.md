# Moving On
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
