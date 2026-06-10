using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickPulse.Explains;
using QuickCheckr;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.FilingCabinet;
using QuickCheckr.Authoring;

namespace QuickTestr.Tests.Tools;

public abstract class TestrTest<T> : QCTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    public class DocTestrAttribute() :
        DocExampleAttribute(typeof(T), nameof(GetTestr));

    public abstract void Example();
    protected abstract void Verify(Article article);

    protected override void ProcessArticle(Article article, string callerPath)
    {
        base.ProcessArticle(article, callerPath);
        Verify(article);
    }

    [StackTraceHidden]
    protected void Run(
        Func<IRecord> testr,
        [CallerFilePath] string callerPath = "")
        => ProcessArticle(TheJournalist.Investigates(testr), callerPath);

    protected abstract ITestrRunner GetTestr();
}
