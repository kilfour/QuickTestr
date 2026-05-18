using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.FilingCabinet;

namespace QuickTestr.Tests.Tools;

public abstract class TestrRunTest<T> : QCTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    [StackTraceHidden]
    protected void Run(
        Func<IRecord> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        var article = TheJournalist.Investigates(runTestr);
        ProcessArticle(article, callerPath);
        verifier(article);
    }
}
