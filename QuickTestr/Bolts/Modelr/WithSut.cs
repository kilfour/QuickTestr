using QuickCheckr;
using QuickFuzzr;

namespace QuickTestr.Bolts.Modelr;

public sealed class WithSut<T, U>(string testName, string fileName, Func<T> model, Func<U> sut)
{
    private readonly Func<T> model = model;
    private readonly Func<U> sut = sut;

    private readonly List<Func<T, U, CheckrOf<Case>>> operations = [];

    public WithSut<T, U> Operation(string label, Action<T> modelOperation, Action<U> sutOperation)
    {
        operations.Add((m, s) => Checkr.Act(label, () => { modelOperation(m); sutOperation(s); }));
        return this;
    }

    public WithSut<T, U> Operation<V>(string label, FuzzrOf<V> fuzzr, Action<T, V> modelOperation, Action<U, V> sutOperation)
    {
        operations.Add((m, s) =>
            from input in Checkr.Input("Input", fuzzr)
            from act in Checkr.Act(label, () => { modelOperation(m, input); sutOperation(s, input); })
            select Case.Closed);
        return this;
    }

    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe)
    {
        if (operations.Count == 0)
            throw new InvalidOperationException("No operations defined. Add at least one .Operation(...) before calling Observe(...).");
        return new(testName, fileName, model, sut, operations, Observation.From(label, observe));
    }

    public WithOperations<T, U> Observe(string label, Func<T, U, bool> observe, Func<ITracer<T, U>, ITracer<T, U>> trace)
    {
        if (operations.Count == 0)
            throw new InvalidOperationException("No operations defined. Add at least one .Operation(...) before calling Observe(...).");
        return new(testName, fileName, model, sut, operations,
            Observation.From(label, observe, ((Tracer<T, U>)trace(new Tracer<T, U>())).TraceCheckr));
    }
}
