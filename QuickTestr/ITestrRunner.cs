using QuickCheckr;
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
