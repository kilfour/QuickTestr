using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickCheckr.Authoring;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.FilingCabinet;
using QuickCheckr.UnderTheHood;

namespace QuickTestr.Tests.Tools;

public abstract class TestrRunTest<T> : QuickCheckrTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    [StackTraceHidden]
    protected void Run(
        Func<ConfiguredCheckr> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        // var article = TheJournalist.Investigates(runTestr);
        // ProcessArticle(article, callerPath);
        // verifier(article);
    }
}
