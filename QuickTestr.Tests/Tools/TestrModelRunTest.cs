using QuickCheckr.Protocol;
using QuickTestr.Bolts.ClerksOffice;

namespace QuickTestr.Tests.Tools;

public abstract class TestrModelRunTest<T> : TestrRunTest<T>
{
    protected override ITranscribe Clerk => new ModelClerk();
}
