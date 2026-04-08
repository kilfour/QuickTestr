using QuickCheckr.UnderTheHood.Proceedings;
using QuickPulse;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrOracleTest<T> : TestrTest<T>
{
    protected override Func<CaseFile, Flow<Flow>> StyleGuide => TheTestr.OracleStyleGuide;
}
