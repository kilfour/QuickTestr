using QuickCheckr;
using QuickCheckr.Authoring.ThePress.Printing;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Tools;
using QuickTestr.Tests.Notes.T_Exploration;

namespace QuickTestr.Tests.Docs.D_ExplorationBasedTesting.Sub;

[DocFile]
[DocFileHeader("Reversing a List")]
public class A_ASimpleExample : TestrPropertyRunTest<A_ASimpleExample>
{
    protected override bool Asserts => false;
    protected override bool Report => false;
    protected override bool Explain => false;


    [Fact(Skip = "WIP")]
    public void Foo()
    {
        var testr =
            Testr.Named("Validation Exploration")
                //.Evil
                .For(Evilr.String())
                .Assert(input => !string.IsNullOrWhiteSpace(input))
                .FillVault(10.Searches(), 20.Runs())
                .Run();
    }
}