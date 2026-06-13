using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.UnderTheHood;

namespace QuickTestr.Tests.Tools;

public abstract class TestrRunTest<T> : TestrBaseTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    [StackTraceHidden]
    protected void Document(
        ITestrRunner testr,
        Action<ITestrRunner> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        var article = Publish(testr, runTestr);
        ProcessArticle(article, callerPath);
        verifier(article);
    }

    protected static Article Publish(ITestrRunner runner, Action<ITestrRunner> runTestr)
    {
        var journalist = new Journalist();
        try { runTestr(runner.StoreCaseFiles(journalist)); }
        catch (FalsifiableException) { }
        return journalist.GetArticle();
    }
}
