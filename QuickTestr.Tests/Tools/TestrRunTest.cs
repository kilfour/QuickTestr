using System.Diagnostics;
using System.Reflection;
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
        Func<ITestrRunner> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        // var article = TheJournalist.Investigates(runTestr);
        // ProcessArticle(article, callerPath);
        // verifier(article);
    }

    [StackTraceHidden]
    protected void Run(
        ITestrRunner testr,
        Action<ITestrRunner> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        var article = Publish(testr, runTestr);
        ProcessArticle(article, callerPath);
        verifier(article);
    }

    public static Article Publish(ITestrRunner runner, Action<ITestrRunner> runTestr)
    {
        var journalist = new Journalist();
        try { runTestr(runner.StoreCaseFiles(journalist)); }
        catch (FalsifiableException) { }
        return journalist.GetArticle();
    }
}
