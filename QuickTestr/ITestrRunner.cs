using QuickCheckr;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickTestr.Bolts;

namespace QuickTestr;

public interface ITestrRunner
{
    CaseFile Run();
    CaseFile Run(int seed);
    CaseFile Run(CheckrOfTRun.RunCount tries);
    void ReviewColdCases();
}

public interface ITestrRunner<TInput>
{
    void GatherEvidence(
        CheckrOfTConductInvestigations.InvestigationCount investigations,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> fingerPrint);

    void GatherEvidence(
        CheckrOfTConductInvestigations.InvestigationCount investigations,
        CheckrOfTRun.RunCount runs,
        Func<TInput, object> fingerPrint,
        Func<InvestigationDirective<TInput>, InvestigationDirective<TInput>> directives);
}