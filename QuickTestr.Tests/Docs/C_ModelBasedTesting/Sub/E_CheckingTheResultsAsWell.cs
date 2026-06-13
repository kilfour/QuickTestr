using QuickCheckr.FilingCabinet;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.C_ModelBasedTesting.Sub;

[DocFile]
[DocModelHeader]
[DocExample(typeof(IdentityCounterModel))]
[DocBoldHeader("SUT")]
[DocExample(typeof(IdentityCounter))]
[DocTestrHeader]
[DocExample(typeof(E_CheckingTheResultsAsWell), nameof(Example))]
[DocReportHeader]
[DocReport]
public class E_CheckingTheResultsAsWell : TestrModelRunTest<E_CheckingTheResultsAsWell>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public void RunExample() => Example();

    [CodeSnippet]
    private static void Example() =>
        Testr.Named("IdentityCounter matches model")
            .Model(() => new IdentityCounterModel())
            .Sut(() => new IdentityCounter())
            .VerifyReturnValues()
            .Operation("Do", Fuzzr.Int(),
                (model, a) => model.Do(a),
                (sut, a) => sut.Do(a))
            .Observe("Counter Matches",
                (model, sut) => model.Counter == sut.Counter)
            .Run();
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