using QuickCheckr.Protocol;
using QuickTestr.Bolts.ClerksOffice;

namespace QuickTestr.Tests.Tools;

public abstract class TestrPropertyRunTest<T> : TestrRunTest<T>
{
    protected override ITranscribe Clerk => new PropertyClerk();
}
