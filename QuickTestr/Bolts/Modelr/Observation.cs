using QuickCheckr;

namespace QuickTestr.Bolts.Modelr;

public record Observation<T, U>(Func<T, U, CheckrOf<Case>> Observe);

public static class Observation
{
    public static Observation<T, U> From<T, U>(string label, Func<T, U, bool> observe, Func<T, U, CheckrOf<Case>>? trace = null)
        => new((m, s) =>
        {
            if (trace is not null)
                return
                    from ok in Checkr.Capture(() => observe(m, s))
                    from tr in Checkr.When(() => !ok, trace(m, s))
                    from ex in Checkr.Expect(label, () => ok)
                    select Case.Closed;
            return Checkr.Expect(label, () => observe(m, s));
        });
}

public record Operation<T, U>(Func<T, U, CheckrOf<Case>> Operate, Func<T, U, CheckrOf<Case>> Trace);
