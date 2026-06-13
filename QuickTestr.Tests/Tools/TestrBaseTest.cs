using QuickCheckr.Authoring;

namespace QuickTestr.Tests.Tools;

public abstract class TestrBaseTest<T> : QuickCheckrTest<T>
{

    protected override bool WriteAllReportsToDisk { get; } = false;
}
