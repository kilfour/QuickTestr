using QuickCheckr.Protocol;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrOracleTest<T> : TestrTest<T>
{
    protected override ITranscribe Clerk => new OracleClerk();
}
