using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickFuzzr;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;

public static class SystemCheckr
{
    public static SystemCheckr<TStashed> For<TStashed>(Func<TStashed> factory) => new(factory);
}

public class SystemCheckr<TStashed>(Func<TStashed> factory)
{
    private string name = string.Empty;
    public SystemCheckr<TStashed> Named(string name)
    {
        this.name = name;
        return this;
    }

    private readonly List<CheckrOf<Case>> pools = [];
    public SystemCheckr<TStashed> PoolOf<T>()
    {
        pools.Add(Trackr.PoolFor<T>());
        return this;
    }
    private Operation<TStashed>[] operations = [];
    public SystemCheckr<TStashed> Operations(params Operation<TStashed>[] operations)
    {
        this.operations = operations;
        return this;
    }

    public void Run() =>
        TheCheckr().Run(5.Runs(), 20.ExecutionsPerRun(), GetConfig());

    public CaseFile Run(CheckrOfTRun.RunCount runs, CheckrOfTRun.ExecutionCount executionsPerRun) =>
        TheCheckr().Run(runs, executionsPerRun, GetConfig());

    public CaseFile Run(int seed, CheckrOfTRun.ExecutionCount executionsPerRun) =>
        TheCheckr().Run(seed, executionsPerRun, GetConfig());

    private CheckrOf<Case> TheCheckr() =>
        from stash in Trackr.Stashed(factory)
        from buildPools in Combine.Checkrs(pools)
        from ops in Checkr.OneOfWhen([.. operations.Select(a => a.Define(stash).Checkr)])
        select Case.Closed;

    private Func<CheckrConfig, CheckrConfig> GetConfig()
    {
        return a => a with
        {
            FileAs = name,
            ReportMode = a.ReportMode & ~ReportMode.Labels & ~ReportMode.StackTrace // & ~ReportMode.Warning
        };
    }
}
