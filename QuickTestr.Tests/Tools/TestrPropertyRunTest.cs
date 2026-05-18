using QuickCheckr.UnderTheHood.Proceedings;
using QuickPulse;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrPropertyRunTest<T> : TestrRunTest<T>
{
    protected override Func<CaseFile, Flow<Flow>> StyleGuide => PropertyStyleGuide.Render;
}
