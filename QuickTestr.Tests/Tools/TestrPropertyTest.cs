using QuickCheckr.UnderTheHood.Proceedings;
using QuickPulse;

namespace QuickTestr.Tests.Tools;

public abstract class TestrPropertyTest<T> : TestrTest<T>
{
    protected override Func<CaseFile, Flow<Flow>> StyleGuide => TheTestr.DefaultStyleGuide;
}
