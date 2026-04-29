using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickPulse;
using QuickTestr.Bolts;

namespace QuickTestr.Tests.Tools;

public abstract class TestrRunTest<T> : QCTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    [StackTraceHidden]
    protected void Run(
        Func<CaseFile> runTestr,
        Action<Article> verifier,
        [CallerFilePath] string callerPath = "")
    {
        var article = TheJournalist.Investigates(runTestr);
        ProcessArticle(article, callerPath);
        verifier(article);
    }
}

public abstract class TestrPropertyRunTest<T> : TestrRunTest<T>
{
    protected override Func<CaseFile, Flow<Flow>> StyleGuide => TheTestr.DefaultStyleGuide;
}

public abstract class TestrOracleRunTest<T> : TestrRunTest<T>
{
    protected override Func<CaseFile, Flow<Flow>> StyleGuide => TheTestr.OracleStyleGuide;
}
