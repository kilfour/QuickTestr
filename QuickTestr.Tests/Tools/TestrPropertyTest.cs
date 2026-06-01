using QuickCheckr.Protocol;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrPropertyTest<T> : TestrTest<T>
{
    protected override ITranscribe Clerk => new PropertyClerk();
}
