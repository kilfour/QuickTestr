using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickPulse.Explains;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.FilingCabinet;
using QuickCheckr.Authoring;
using QuickCheckr.UnderTheHood;
using System.Reflection;

namespace QuickTestr.Tests.Tools;

public abstract class TestrTest<T> : QuickCheckrTest<T>
{
    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    protected class DocTestrAttribute() :
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
        Func<ITestrRunner> testr,
        [CallerFilePath] string callerPath = "")
    { }//ProcessArticle(TheJournalist.Investigates(testr), callerPath);

    [StackTraceHidden]
    protected void Document(
        Action<ITestrRunner> runTestr,
        [CallerFilePath] string callerPath = "")
    {
        var testr = GetTestr();
        var methodInfo = typeof(ITestrRunner).GetMethod("AddFileAsToConfig", BindingFlags.NonPublic);
        // CheckrOf<Case> checkr,
        // Action< ConfiguredCheckr > runCheckr)

        // try
        // {
        //     runCheckr(checkr.Configure(a => a with { FileAs = "WhistleBlower", Custodian = this }));
        // }
        // catch (FalsifiableException) { }

        // var article = Journalist.Publish(GetTestr(), runCheckr);
        // ProcessArticle(article, callerPath);
        // Verify(article);
    }

    protected abstract ITestrRunner GetTestr();
}
