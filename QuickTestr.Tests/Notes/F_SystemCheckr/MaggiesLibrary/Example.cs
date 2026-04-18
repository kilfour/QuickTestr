using QuickPulse.Explains;
using QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;
using QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary.Domain;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary;

[DocFile]
[DocExample(typeof(Example), nameof(SchoolRegistryTestr))]
[DocExample(typeof(RegisterStudent))]
[DocExample(typeof(StudentAddOrUpdateScore))]
public class Example
{
    [Fact]
    [CodeSnippet]
    public void SchoolRegistryTestr() =>
        SystemCheckr
            .For(() => new SchoolRegistry())
            .Named("Maggie's Library")
            .PoolOf<StudentInfo>()
            .Operations(
                new RegisterStudent(),
                new StudentAddOrUpdateScore())
            .Run();
}


