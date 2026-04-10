using System.Diagnostics;
using QuickCheckr;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood.Proceedings;


namespace QuickTestr.Bolts;

public abstract class BaseTestrRunner<TInput> : ITestrRunner, ITestrRunner<TInput>
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

    public void GatherEvidence(
        CheckrOfTConductInvestigations.InvestigationCount investigations,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> fingerPrint,
        Func<InvestigationDirective<TInput>, InvestigationDirective<TInput>> directives)
    {
        var configuredDirectives = directives(new InvestigationDirective<TInput>(5, null));
        GetCheckr().GatherEvidence(
                investigations,
                runs,
                1.ExecutionsPerRun(),
                AddFileAsToConfig(),
                (a) => fingerPrint(a.GetInput<TInput>("Input")),
                configuredDirectives.MaxCaseFiles,
                configuredDirectives.RejectWhen != null ? a => configuredDirectives.RejectWhen(a.GetInput<TInput>("Input")) : null);
    }

    public void GatherEvidence(
        CheckrOfTConductInvestigations.InvestigationCount investigations,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> fingerPrint)
            => GatherEvidence(investigations, runs, fingerPrint, a => a);

    public void ReviewColdCases()
        => GetCheckr().ReviewColdCases(AddFileAsToConfig());

    private Func<CheckrConfig, CheckrConfig> AddFileAsToConfig()
        => a => GetConfig()(a) with { FileAs = TestName };

    public void CloseResolvedColdCases()
        => GetCheckr().CloseResolvedColdCases(AddFileAsToConfig());

    protected abstract CheckrOf<Case> GetCheckr();
    protected abstract Func<CheckrConfig, CheckrConfig> GetConfig();
    public abstract string TestName { get; }
}
