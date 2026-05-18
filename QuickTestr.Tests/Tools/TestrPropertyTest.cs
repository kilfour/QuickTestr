using QuickCheckr.FilingCabinet;
using QuickPulse;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrPropertyTest<T> : TestrTest<T>
{
    protected override Func<IRecord, Flow<Flow>> StyleGuide => PropertyStyleGuide.Render;
}
