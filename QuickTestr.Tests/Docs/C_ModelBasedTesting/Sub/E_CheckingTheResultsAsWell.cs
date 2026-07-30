using QuickCheckr;
using QuickCheckr.Authoring.ThePress;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Bolts.Builders.ModelBased;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.C_ModelBasedTesting.Sub;

[DocFile]
[DocModelHeader]
[DocExample(typeof(IdentityCounterModel))]
[DocBoldHeader("SUT")]
[DocExample(typeof(IdentityCounter))]
[DocTestrHeader]
[DocTestr]
[DocReportHeader]
[DocReport]
public class E_CheckingTheResultsAsWell : QuickTestrModelRunTest<E_CheckingTheResultsAsWell>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public override void Example() => Document();

    [CodeSnippet]
    [CodeRemoveJournalist]
    protected override void GetTestr(Journalist journalist) =>
        Testr.Named("IdentityCounter matches model")
            .Model(() => new IdentityCounterModel())
            .Sut(() => new IdentityCounter())
            .VerifyReturnValues()
            .Operation("Do", Fuzzr.Int(),
                (model, a) => model.Do(a),
                (sut, a) => sut.Do(a))
            .Observe("Counter Matches",
                (model, sut) => model.Counter == sut.Counter)
            .StoreCaseFiles(journalist)
            .Run();

    protected override void Verify(Article article)
    {
    }
}

[CodeExample]
public class IdentityCounterModel
{
    public int Counter { get; private set; }
    public int Do(int a)
    {
        Counter++;
        return a;
    }
}

[CodeExample]
public class IdentityCounter
{
    public int Counter { get; private set; }
    public int Do(int a)
    {
        Counter++;
        if (Counter > 3)
            return 0;
        return a;
    }
}
