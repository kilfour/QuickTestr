using QuickCheckr.FilingCabinet;
using QuickPulse;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrOracleTest<T> : TestrTest<T>
{
    protected override Func<IRecord, Flow<Flow>> StyleGuide => OracleStyleGuide.Render;
}
