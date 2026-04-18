using QuickCheckr;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickPulse.Bolts;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;


public interface IOperationFailure<TInput>
{
    public CheckrOf<Case> GetCheckr(TInput input);
}

public record OperationFailure<TPoolElement, TInput>(
    string Label,
    Func<TInput, TInput> Input,
    Action<TInput> Action,
    Func<DelayedResult, bool> Expectation) : IOperationFailure<TInput>
{
    public CheckrOf<Case> GetCheckr(TInput input) =>
        from flag in Trackr.Stashed(Label, () => new Cell<bool>(true))
        from maybe in Checkr.When(() => flag.Value,
            from act in Checkr.ActCarefully(Label, () => Action(Input(input)))
            from checks in Checkr.Expect($"{Label} Fails", () => Expectation(act))
            from flip in Checkr.Perform(() => flag.Value = !flag.Value)
            select Case.Closed)
        select Case.Closed;
}

public interface IPoolOperationFailure<TPoolElement, TInput>
{
    public CheckrOf<Case> GetCheckr(TPoolElement element, TInput input);
}

public record PoolOperationFailure<TPoolElement, TInput>(
    string Label,
    Func<TInput, TInput> Input,
    Action<TPoolElement, TInput> Action,
    Func<DelayedResult, bool> Expectation) : IPoolOperationFailure<TPoolElement, TInput>
{
    public CheckrOf<Case> GetCheckr(TPoolElement element, TInput input) =>
        from flag in Trackr.Stashed(Label, () => new Cell<bool>(true))
        from maybe in Checkr.When(() => flag.Value,
            from act in Checkr.ActCarefully(Label, () => Action(element, Input(input)))
            from checks in Checkr.Expect($"{Label} Fails", () => Expectation(act))
            from flip in Checkr.Perform(() => flag.Value = !flag.Value)
            select Case.Closed)
        select Case.Closed;
}