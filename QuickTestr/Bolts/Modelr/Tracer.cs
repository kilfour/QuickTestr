using QuickCheckr;
using QuickCheckr.UnderTheHood;

namespace QuickTestr.Bolts.Modelr;

public interface ITracer<T, U>
{
    Tracer<T, U> Trace();
    Tracer<T, U> Trace<V, W>(Func<T, U, (V, W)> projector);
}

public class Tracer<T, U> : ITracer<T, U>
{
    public Func<T, U, CheckrOf<Case>> TraceCheckr { get; private set; } = (m, s) => s => CheckrResult.CaseOnly(s);

    public Tracer<T, U> Trace() => Trace((m, s) => (m, s));

    public Tracer<T, U> Trace<V, W>(Func<T, U, (V, W)> projector)
    {
        TraceCheckr =
            (m, s) =>
            {
                var result = projector(m, s);

                return
                    from modelTrace in Checkr.Trace("Model", () => result.Item1)
                    from sutTrace in Checkr.Trace("Sut", () => result.Item2)
                    select Case.Closed;
            };
        return this;
    }
}
