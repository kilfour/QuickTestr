using QuickCheckr;
using QuickFuzzr;

namespace QuickTestr.Bolts.Modelr;

/// <summary>
/// Configures operations and observations for a model-based Testr.
/// Use after supplying both the model and the system under test.
/// </summary>
public sealed class WithSut<T, U>(string testName, string fileName, bool useBuiltInReducers, CheckrOf<Case>[] formatters, Func<T> model, Func<U> sut)
{
    private readonly Func<T> model = model;
    private readonly Func<U> sut = sut;

    private readonly List<Func<T, U, CheckrOf<Case>>> operations = [];

    /// <summary>
    /// Adds a state transition that runs on both the model and the system under test.
    /// Use when the operation needs no generated input.
    /// </summary>
    public WithSut<T, U> Operation(string label, Action<T> modelOperation, Action<U> sutOperation)
    {
        operations.Add((m, s) => Checkr.Act(label, () => { modelOperation(m); sutOperation(s); }));
        return this;
    }

    /// <summary>
    /// Adds a generated state transition that runs on both the model and the system under test.
    /// Use when the operation should explore parameterized inputs.
    /// </summary>
    public WithSut<T, U> Operation<V>(string label, FuzzrOf<V> fuzzr, Action<T, V> modelOperation, Action<U, V> sutOperation)
    {
        operations.Add((m, s) =>
            from input in Checkr.Input("Input", fuzzr)
            from act in Checkr.Act(label, () => { modelOperation(m, input); sutOperation(s, input); })
            select Case.Closed);
        return this;
    }

    /// <summary>
    /// Adds the first observation for the configured operations.
    /// Use when you are ready to assert an invariant between the model and the system under test.
    /// </summary>
    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe)
    {
        if (operations.Count == 0)
            throw new InvalidOperationException("No operations defined. Add at least one .Operation(...) before calling Observe(...).");
        return new(testName, fileName, useBuiltInReducers, formatters, model, sut, operations, Observation.From(label, observe));
    }

    /// <summary>
    /// Adds the first observation with custom trace output for the configured operations.
    /// Use when a model-based invariant needs targeted diagnostics on failure.
    /// </summary>
    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe, Func<ITracer<T, U>, ITracer<T, U>> trace)
    {
        if (operations.Count == 0)
            throw new InvalidOperationException("No operations defined. Add at least one .Operation(...) before calling Observe(...).");
        return new(testName, fileName, useBuiltInReducers, formatters, model, sut, operations,
            Observation.From(label, observe, ((Tracer<T, U>)trace(new Tracer<T, U>())).TraceCheckr));
    }
}
