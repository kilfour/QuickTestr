using QuickCheckr.Protocol;
using QuickTestr.Bolts;
using QuickTestr.Bolts.ClerksOffice;

namespace QuickTestr.Tests.Tools;

public abstract class TestrPropertyTest<T> : TestrTest<T>
{
    protected override ITranscribe Clerk => new PropertyClerk();
}
