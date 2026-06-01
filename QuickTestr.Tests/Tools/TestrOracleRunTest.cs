using QuickCheckr.Protocol;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrOracleRunTest<T> : TestrRunTest<T>
{
    protected override ITranscribe Clerk => new OracleClerk();
}
