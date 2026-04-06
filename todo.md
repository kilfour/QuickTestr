## Next version: 0.0.2 : ...
 
### Doing

### Todo

### Wish List

### Done/Ready for Changelog after review

## 0.0.2 : ...

## Skipped Tests

## Future Doc Work

## Ideas

## Refactoring Targets 

### Already Considered

## Other


Testr
    .Named("Interpreter matches golden model.")
    .For(ExpressionFuzzr)
    .Expected(a => LostIn.Translation(a).Eval())
    .Actual(a => LostIn.FaultyTranslation(a).Eval());