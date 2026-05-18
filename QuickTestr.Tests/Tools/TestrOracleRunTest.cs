using QuickCheckr.FilingCabinet;
using QuickPulse;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrOracleRunTest<T> : TestrRunTest<T>
{
    protected override Func<IRecord, Flow<Flow>> StyleGuide => OracleStyleGuide.Render;
}
