using QuickCheckr;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;

namespace QuickTestr.Bolts.Builders.ModelBased;

/// <summary>
/// Configures operations and observations for a model-based Testr.
/// Use after supplying both the model and the system under test.
/// </summary>
public sealed class WithSut<T, U>(string testName, bool useBuiltInReducers, CheckrOf<Case>[] formatters, Func<T> model, Func<U> sut)
{
    private readonly Func<T> model = model;
    private readonly Func<U> sut = sut;

    private readonly List<Func<bool, T, U, CheckrOf<(Func<bool> condition, CheckrOf<Case> checkr)>>> operations = [];

    private WithSut<T, U> AddOperation(Func<bool, T, U, CheckrOf<Case>> operation)
    {
        operations.Add((a, m, s) => Checkr.Option(() => true, operation(a, m, s)));
        return this;
    }

    private WithSut<T, U> AddConditionalOperation(
        Func<T, bool> condition,
        Func<bool, T, U, CheckrOf<Case>> operation)
    {
        operations.Add((a, m, s) =>
            Checkr.Option(() => condition(m), operation(a, m, s)));
        return this;
    }

    private bool verifyReturnValues;
    /// <summary>
    /// Enables result verification for operations that return values.
    /// Use before adding return-value operations when the model and the system under test should be compared.
    /// </summary>
    public WithSut<T, U> VerifyReturnValues()
    {
        verifyReturnValues = true;
        return this;
    }

    private static bool CheckResults<TResult>(DelayedResult<TResult> expected, DelayedResult<TResult> actual)
    {
        if (!expected.Threw && !actual.Threw)
        {
            if (expected.HasValue != actual.HasValue)
                return false;
            if (!expected.HasValue)
                return true;
            return Equals(expected.Value, actual.Value);
        }
        if (expected.Threw && actual.Threw)
            return ExceptionIs.Equivalent(expected.Exception, actual.Exception);
        return false;
    }

    private static bool CheckResults(DelayedResult expected, DelayedResult actual)
    {
        if (!expected.Threw && !actual.Threw)
            return true;
        if (expected.Threw && actual.Threw)
            return ExceptionIs.Equivalent(expected.Exception, actual.Exception);
        return false;
    }

    private static string GetExceptionReport(Exception exception)
        => $"{exception!.GetType().Name}: {exception.Message}";

    /// <summary>
    /// Adds a state transition that runs on both the model and the system under test.
    /// Use when the operation needs no generated input.
    /// </summary>
    public WithSut<T, U> Operation(string label, Action<T> modelOperation, Action<U> sutOperation)
        => AddOperation((a, m, s) =>
            from act in Checkr.Act(label, () => { modelOperation(m); sutOperation(s); })
            select Case.Closed);

    /// <summary>
    /// Adds a conditional state transition that runs on both the model and the system under test.
    /// Use when the operation needs no generated input and should only run in specific model states.
    /// </summary>
    public WithSut<T, U> Operation(string label, Func<T, bool> condition, Action<T> modelOperation, Action<U> sutOperation)
        => AddConditionalOperation(condition, (a, m, s) =>
             from act in Checkr.ActCarefully(label, () => { modelOperation(m); sutOperation(s); })
             select Case.Closed);

    /// <summary>
    /// Adds a generated state transition that runs on both the model and the system under test.
    /// Use when the operation should explore parameterized inputs.
    /// </summary>
    public WithSut<T, U> Operation<V>(string label, FuzzrOf<V> fuzzr, Action<T, V> modelOperation, Action<U, V> sutOperation)
        => AddOperation((a, m, s) =>
            from input in Checkr.Input("Input", fuzzr)
            from modelResult in Checkr.ActCarefully($"{label} Model", () => modelOperation(m, input))
            from sutResult in Checkr.ActCarefully($"{label} Sut", () => sutOperation(s, input))
            from checkResult in Checkr.When(() => a,
                from traceExpectedException in Checkr.TraceWhen("Expected", () => modelResult.Threw, () => GetExceptionReport(modelResult.Exception!))
                from traceActualException in Checkr.TraceWhen("Actual  ", () => sutResult.Threw, () => GetExceptionReport(sutResult.Exception!))
                from expectation in Checkr.Expect($"{label}, results do not match", () => CheckResults(modelResult, sutResult))
                select Case.Closed)
            select Case.Closed);

    /// <summary>
    /// Adds a generated state transition that runs on both the model and the system under test.
    /// Use when the operation should explore parameterized inputs.
    /// </summary>
    public WithSut<T, U> Operation<V, W>(string label, FuzzrOf<V> fuzzr, Func<T, V, W> modelOperation, Func<U, V, W> sutOperation)
        => AddOperation((a, m, s) =>
            from input in Checkr.Input("Input", fuzzr)
            from modelResult in Checkr.ActCarefully($"{label} Model", () => modelOperation(m, input))
            from sutResult in Checkr.ActCarefully($"{label} Sut", () => sutOperation(s, input))
            from checkResult in Checkr.When(() => a,
                from traceExpectedValue in Checkr.TraceWhen("Expected", () => !modelResult.Threw, () => modelResult.Value)
                from traceExpectedException in Checkr.TraceWhen("Expected", () => modelResult.Threw, () => GetExceptionReport(modelResult.Exception!))
                from traceActualValue in Checkr.TraceWhen("Actual  ", () => !sutResult.Threw, () => sutResult.Value)
                from traceActualException in Checkr.TraceWhen("Actual  ", () => sutResult.Threw, () => GetExceptionReport(sutResult.Exception!))
                from expectation in Checkr.Expect($"{label}, results do not match", () => CheckResults(modelResult, sutResult))
                select Case.Closed)
            select Case.Closed);

    /// <summary>
    /// Adds a conditional generated state transition that runs on both the model and the system under test.
    /// Use when the operation should explore parameterized inputs only in specific model states.
    /// </summary>
    public WithSut<T, U> Operation<V>(string label, Func<T, bool> condition, FuzzrOf<V> fuzzr, Action<T, V> modelOperation, Action<U, V> sutOperation)
        => AddConditionalOperation(condition, (a, m, s) =>
            from input in Checkr.Input("Input", fuzzr)
            from act in Checkr.ActCarefully(label, () => { modelOperation(m, input); sutOperation(s, input); })
            select Case.Closed);

    /// <summary>
    /// Adds a generated state transition that runs on both the model and the system under test.
    /// Use when the generated input should depend on the current model state.
    /// </summary>
    public WithSut<T, U> Operation<V>(string label, Func<T, FuzzrOf<V>> fuzzr, Action<T, V> modelOperation, Action<U, V> sutOperation)
        => AddOperation((a, m, s) =>
            from input in Checkr.Input("Input", () => fuzzr(m))
            from act in Checkr.ActCarefully(label, () => { modelOperation(m, input); sutOperation(s, input); })
            select Case.Closed);

    /// <summary>
    /// Adds a conditional generated state transition that runs on both the model and the system under test.
    /// Use when the generated input should depend on the current model state and only run in specific model states.
    /// </summary>
    public WithSut<T, U> Operation<V>(string label, Func<T, bool> condition, Func<T, FuzzrOf<V>> fuzzr, Action<T, V> modelOperation, Action<U, V> sutOperation)
        => AddConditionalOperation(condition, (a, m, s) =>
            from input in Checkr.Input("Input", () => fuzzr(m))
            from act in Checkr.ActCarefully(label, () => { modelOperation(m, input); sutOperation(s, input); })
            select Case.Closed);


    /// <summary>
    /// Adds the first observation for the configured operations.
    /// Use when you are ready to assert an invariant between the model and the system under test.
    /// </summary>
    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe)
    {
        if (operations.Count == 0)
            throw new InvalidOperationException("No operations defined. Add at least one .Operation(...) before calling Observe(...).");
        return new(testName, useBuiltInReducers, formatters, model, sut, verifyReturnValues, operations, Observation.From(label, observe));
    }

    /// <summary>
    /// Adds the first observation with custom trace output for the configured operations.
    /// Use when a model-based invariant needs targeted diagnostics on failure.
    /// </summary>
    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe, Func<ITracer<T, U>, ITracer<T, U>> trace)
    {
        if (operations.Count == 0)
            throw new InvalidOperationException("No operations defined. Add at least one .Operation(...) before calling Observe(...).");
        return new(testName, useBuiltInReducers, formatters, model, sut, verifyReturnValues, operations,
            Observation.From(label, observe, ((Tracer<T, U>)trace(new Tracer<T, U>())).TraceCheckr));
    }
}
