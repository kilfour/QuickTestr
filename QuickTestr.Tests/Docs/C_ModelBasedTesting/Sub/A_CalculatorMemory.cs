using QuickCheckr.FilingCabinet;
using QuickCheckr.UnderTheHood;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.C_ModelBasedTesting.Sub;

[DocFile]
[DocModelHeader]
[DocExample(typeof(CalculatorModel))]
[DocBoldHeader("SUT")]
[DocExample(typeof(Calculator))]
[DocTestrHeader]
[DocExample(typeof(A_CalculatorMemory), nameof(Example))]
[DocReportHeader]
[DocReport]
public class A_CalculatorMemory : TestrModelRunTest<A_CalculatorMemory>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;

    [Fact]
    public void RunExample() => Example();

    [CodeSnippet]
    private static void Example() =>
        Testr.Named("Calculator Clear matches model")
            .Model(() => new CalculatorModel())
            .Sut(() => new Calculator())
            .Operation("Add", Fuzzr.Int(),
                (model, a) => model.Add(a),
                (sut, a) => sut.Add(a))
            .Operation("Subtract", Fuzzr.Int(),
                (model, a) => model.Subtract(a),
                (sut, a) => sut.Subtract(a))
            .Operation("Clear",
                model => model.Clear(),
                sut => sut.Clear())
            .Observe("Result Matches",
                (model, sut) => model.Result == sut.Result, a => a.Trace())
            .Run();
}


[CodeExample]
public class CalculatorModel
{
    public int Result { get; private set; } = 0;
    public void Add(int a) => Result += a;
    public void Subtract(int a) => Result -= a;
    public void Clear() => Result = 0;
}

[CodeExample]
public class Calculator
{
    public int Result { get; private set; } = 0;
    private int counter = 0;
    public void Add(int a)
    {
        counter++;
        Result += a;
    }

    public void Subtract(int a) => Result -= a;
    public void Clear()
    {
        if (counter != 3)
            Result = 0;
    }
}