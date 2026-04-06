using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood.Proceedings;

namespace QuickTestr;

public interface ITestrRunner
{
    CaseFile Run();
    CaseFile Run(int seed);
    CaseFile Run(CheckrOfTRun.RunCount tries);
    void GatherEvidence(CheckrOfTGatherEvidence.InvestigationCount investigations, CheckrOfTRun.RunCount runs, Func<ICaseFileSummary, int>? fingerprint = null);
    void ReviewColdCases();
}

public abstract class BaseTestrRunner : ITestrRunner
{
    [StackTraceHidden]
    public CaseFile Run()
        => Run(100.Runs());

    [StackTraceHidden]
    public CaseFile Run(int seed)
        => GetCheckr().Run(seed, GetConfig());

    [StackTraceHidden]
    public CaseFile Run(CheckrOfTRun.RunCount tries)
        => GetCheckr().Run(tries, GetConfig());

    public void GatherEvidence(CheckrOfTGatherEvidence.InvestigationCount investigations, CheckrOfTRun.RunCount runs, Func<ICaseFileSummary, int>? fingerprint = null)
        => GetCheckr().GatherEvidence(
            investigations,
            runs,
            1.ExecutionsPerRun(),
            a => GetConfig()(a) with { FileAs = TestName },
            fingerprint);

    public void ReviewColdCases()
        => GetCheckr().ReviewColdCases(a => GetConfig()(a) with { FileAs = TestName });

    protected abstract CheckrOf<Case> GetCheckr();
    protected abstract Func<CheckrConfig, CheckrConfig> GetConfig();
    public abstract string TestName { get; }
}
