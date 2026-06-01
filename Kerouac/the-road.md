## Next version: 0.0.7 : This Year's Model
 
### Doing


### Todo
* Remove duplication in the runners.
* reorganize per feature
* mbt report
  ```text
  Falsified after 13 executions
  Minimal scenario: 4 executions
  Seed: 1636527909
  
  Scenario
  1. Add(64)
  2. Add(_)
  3. Add(_)
  4. Clear()
     !! ComputerSaysNo: ... tired now ...
  
  State at failure
  Model: { Result: 0 }
  SUT:   { Result: 64 }
  
  Observations
  - Result Matches: passed 12x

  Coverage
  - Add: executed 3x
  - Subtract: removed by shrinker
  - Clear: executed 1x
  ```

============================================================
 No two different indexes point to each other.
============================================================
 Investigations run: 100
 Distinct failures kept: 7
 Similar failures skipped: 34

 Failures:
 1. Input = [ 1, 0 ]
    Seeds: 1934478623, 65441555, ...
    Occurrences: 12

 2. Input = [ 2, 0, 1 ]
    Seeds: ...
    Occurrences: 5

 3. Input = [ 3, 2, 1, 0 ]
    Seeds: ...
    Occurrences: 2
```

### Wish List
* Auto DeliberationPolicy for Testr
  Derive default deliberation policy from input type
  
### Done/Ready for Changelog after review
* Upgraded QuickCheckr dependency, taking advantage of the new Reporting `Clerk`.

## 0.0.8 : ...

## Skipped Tests

## Future Doc Work

## Ideas

## Refactoring Targets 

### Already Considered

## Other
