using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.Protocol;
using QuickCheckr.UnderTheHood;
using QuickTestr.Bolts.Builders.ModelBased;
using QuickTestr.Bolts.ClerksOffice;

namespace QuickTestr.Tests.Tools;

public abstract class TestrModelRunTest<T> : TestrRunTest<T>
{
    protected override ITranscribe Clerk => new ModelClerk();

    [StackTraceHidden]
    protected void Document(
        IModelrRunner testr,
        Action<IModelrRunner> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        var article = Publish(testr, runTestr);
        ProcessArticle(article, callerPath);
        verifier(article);
    }

    protected static Article Publish(IModelrRunner runner, Action<IModelrRunner> runTestr)
    {
        var journalist = new Journalist();
        try { runTestr(runner.StoreCaseFiles(journalist)); }
        catch (FalsifiableException) { }
        return journalist.GetArticle();
    }
}
