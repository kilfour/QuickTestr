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
        var configuredDirectives = directives(new InvestigationDirective<TInput>(null, null));
        GetCheckr().GatherEvidence(
                investigations,
                runs,
                1.ExecutionsPerRun(),
                a => GetConfig()(a) with { FileAs = TestName },
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
        => GetCheckr().ReviewColdCases(a => GetConfig()(a) with { FileAs = TestName });

    public void CloseResolvedColdCases()
        => GetCheckr().CloseResolvedColdCases(a => GetConfig()(a) with { FileAs = TestName });

    protected abstract CheckrOf<Case> GetCheckr();
    protected abstract Func<CheckrConfig, CheckrConfig> GetConfig();
    public abstract string TestName { get; }
}
