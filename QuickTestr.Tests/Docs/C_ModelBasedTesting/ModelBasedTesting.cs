using QuickCheckr;
using QuickCheckr.Authoring;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickCheckr.UnderTheHood.Proceedings;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;

namespace QuickTestr.Tests.Docs.C_ModelBasedTesting;

[DocFile]
[DocModelHeader]
[DocExample(typeof(CalculatorModel))]
[DocBoldHeader("SUT")]
[DocExample(typeof(Calculator))]
[DocTestrHeader]
[DocExample(typeof(ModelBasedTesting), nameof(Example))]
[DocReportHeader]
[DocReport]
public class ModelBasedTesting : TestrRunTest<ModelBasedTesting>
{
    protected override bool Asserts => true;
    protected override bool Report => true;
    protected override bool Explain => false;

    [Fact]
    public void RunExample() => Run(Example, a => { });

    [CodeSnippet]
    private static CaseFile Example() =>
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
            //a => a.Trace((model, sut) => (model.Result, sut.Result)))
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