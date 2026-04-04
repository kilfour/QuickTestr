using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuickTestr.Tests.Tools.ThePress;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickPulse;
using QuickPulse.Explains;
using QuickCheckr;

namespace QuickTestr.Tests.Tools;

public abstract class TestrTest<T> : QCTest<T>
{
    protected override Func<CaseFile, Flow<Flow>> StyleGuide => TheTestr.DefaultStyleGuide;

    protected class DocTestrHeaderAttribute() :
        DocBoldHeaderAttribute("The Testr");

    public class DocTestrAttribute() :
        DocExampleAttribute(typeof(T), nameof(GetTestr));

    [StackTraceHidden]
    protected void Run(
        [CallerFilePath] string callerPath = "")
        => ProcessArticle(TheJournalist.Exposes(
            () => GetTestr().Run())
            , callerPath);

    protected void Run(
        int seed,
        [CallerFilePath] string callerPath = "")
        => ProcessArticle(TheJournalist.Exposes(
            () => GetTestr().Run(seed))
            , callerPath);

    [StackTraceHidden]
    protected void Run(
        CheckrOfTRun.RunCount runCount,
        [CallerFilePath] string callerPath = "")
        => ProcessArticle(TheJournalist.Exposes(
            () => GetTestr().Run(runCount))
            , callerPath);


    // [StackTraceHidden]
    // protected void RunWithoutFailure(
    //     [CallerFilePath] string callerPath = "")
    //     => ProcessArticle(TheJournalist.Unearths(TheInvariant())
    //         , callerPath);

    protected abstract ITestrRunner GetTestr();
}
