using QuickCheckr;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;

public class OperationBuilder(string name)
{
    public OperationBuilder<TPoolElement> When<TPoolElement>(Func<IReadOnlyCollection<TPoolElement>, bool> predicate)
        => new(name, predicate);
}

public class OperationBuilder<TPoolElement>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate)
{
    public OperationBuilder<TPoolElement, TInput> With<TInput>(FuzzrOf<TInput> fuzzr)
        => new(name, predicate, fuzzr);
}

public class OperationBuilder<TPoolElement, TInput>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr)
{
    public OperationBuilderWithAction<TPoolElement, TInput> Perform(Action<TInput> perform)
        => new(name, predicate, fuzzr, perform);

    public OperationBuilderWithActionAndPoolElement<TPoolElement, TInput> Perform(Action<TPoolElement, TInput> perform)
        => new(name, predicate, fuzzr, perform);
}

public class OperationBuilderWithActionAndPoolElement<TPoolElement, TInput>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr,
    Action<TPoolElement, TInput> perform)
{
    public OperationBuilderWithActionAndPoolElementStored<TPoolElement, TInput> Store(
        Func<TPoolElement, TInput, TPoolElement> store)
        => new(name, predicate, fuzzr, perform, store);
}

public class OperationBuilderWithActionAndPoolElementStored<TPoolElement, TInput>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr,
    Action<TPoolElement, TInput> perform,
    Func<TPoolElement, TInput, TPoolElement> store)
{
    public OperationBuilderWithEntityAndPoolElement<TPoolElement, TInput, TEntity> Load<TEntity>(
        Func<TPoolElement, TEntity> loadEntity)
        => new(name, predicate, fuzzr, perform, store, loadEntity);
}

public class OperationBuilderWithAction<TPoolElement, TInput>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr,
    Action<TInput> perform)
{
    public OperationBuilderWithPoolElementStored<TPoolElement, TInput> Store(Func<TInput, TPoolElement> getPoolElement)
        => new(name, predicate, fuzzr, perform, getPoolElement);
}

public class OperationBuilderWithPoolElementStored<TPoolElement, TInput>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr,
    Action<TInput> perform,
    Func<TInput, TPoolElement> getPoolElement)
{
    public OperationBuilderWithEntity<TPoolElement, TInput, TEntity> Load<TEntity>(
        Func<TPoolElement, TEntity> loadEntity)
        => new(name, predicate, fuzzr, perform, getPoolElement, loadEntity);
}

public class OperationBuilderWithEntity<TPoolElement, TInput, TEntity>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr,
    Action<TInput> perform,
    Func<TInput, TPoolElement> getPoolElement,
    Func<TPoolElement, TEntity> loadEntity)
{
    private readonly List<IOperationFailure<TInput>> examples = [];

    public OperationBuilderWithEntity<TPoolElement, TInput, TEntity> FailsWith<TException>(
        string exampleLabel,
        Func<TInput, TInput> inputValue) where TException : Exception
    {
        examples.Add(new OperationFailure<TPoolElement, TInput>(
            $"'{name}' {exampleLabel}", inputValue, perform, a => a.ThrewExactly<TException>()));
        return this;
    }
    public Specification Expect(
        params (string label, Func<TPoolElement, TEntity, bool> expectation)[] expectations) =>
        new(GetCheckr(expectations));

    private CheckrOf<(Func<bool> condition, CheckrOf<Case> checkr)> GetCheckr(
        (string label, Func<TPoolElement, TEntity, bool> expectation)[] expectations) =>
        Trackr.PoolWhen(predicate,
            from input in Checkr.Input($"'{name}' Input", fuzzr)
            from act in Checkr.Act(name, () => perform(input))
            from element in Checkr.Capture(() => getPoolElement(input))
            from entity in Checkr.Capture(() => loadEntity(element))
            from exists in Checkr.Expect($"'{name}' Load succeeded.", () => entity != null)
            from storeElement in Trackr.ToPool($"'{name}' to Pool", element)
            from checks in Combine.Checkrs(expectations.Select(a =>
                Checkr.Expect($"'{name}' {a.label}", () => a.expectation(element, entity))))
            from checkExamples in Combine.Checkrs(examples.Select(e => e.GetCheckr(input)))
            select Case.Closed);
}

public class OperationBuilderWithEntityAndPoolElement<TPoolElement, TInput, TEntity>(
    string name,
    Func<IReadOnlyCollection<TPoolElement>, bool> predicate,
    FuzzrOf<TInput> fuzzr,
    Action<TPoolElement, TInput> perform,
    Func<TPoolElement, TInput, TPoolElement> store,
    Func<TPoolElement, TEntity> loadEntity)
{
    private readonly List<IPoolOperationFailure<TPoolElement, TInput>> examples = [];

    public OperationBuilderWithEntityAndPoolElement<TPoolElement, TInput, TEntity> FailsWith<TException>(
        string exampleLabel,
        Func<TInput, TInput> inputValue) where TException : Exception
    {
        examples.Add(new PoolOperationFailure<TPoolElement, TInput>(
            $"'{name}' {exampleLabel}", inputValue, perform, a => a.ThrewExactly<TException>()));
        return this;
    }
    public Specification Expect(
        params (string label, Func<TPoolElement, TEntity, bool> expectation)[] expectations) =>
        new(GetCheckr(expectations));

    private CheckrOf<(Func<bool> condition, CheckrOf<Case> checkr)> GetCheckr(
        (string label, Func<TPoolElement, TEntity, bool> expectation)[] expectations) =>
        Trackr.PoolWhen(name, predicate, element =>
            from input in Checkr.Input($"'{name}' Input", fuzzr)
            from act in Checkr.Act(name, () => perform(element.Value, input))
            from entity in Checkr.Capture(() => loadEntity(element.Value))
            from exists in Checkr.Expect($"'{name}' Load succeeded.", () => entity != null)
            from storeElement in element.Replace(store(element.Value, input))
            from checks in Combine.Checkrs(expectations.Select(a =>
                Checkr.Expect($"'{name}' {a.label}", () => a.expectation(element.Value, entity))))
            from checkExamples in Combine.Checkrs(examples.Select(e => e.GetCheckr(element.Value, input)))
            select Case.Closed);
}
