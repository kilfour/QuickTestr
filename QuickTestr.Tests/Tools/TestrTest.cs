using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickPulse.Explains;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.UnderTheHood;

namespace QuickTestr.Tests.Tools;

public abstract class TestrTest<T> : TestrBaseTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    protected class DocTestrAttribute() :
        DocExampleAttribute(typeof(T), nameof(GetTestr));

    public abstract void Example();
    protected abstract void Verify(Article article);

    protected abstract ITestrRunner GetTestr();

    [StackTraceHidden]
    protected void Document(
        Action<ITestrRunner> runTestr,
        [CallerFilePath] string callerPath = "")
    {
        var article = Publish(GetTestr(), runTestr);
        ProcessArticle(article, callerPath);
        Verify(article);
    }

    private static Article Publish(ITestrRunner runner, Action<ITestrRunner> runTestr)
    {
        var journalist = new Journalist();
        try { runTestr(runner.StoreCaseFiles(journalist)); }
        catch (FalsifiableException) { }
        return journalist.GetArticle();
    }
}
