using QuickCheckr;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;
using QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary.Domain;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary;

[CodeExample]
public class RegisterStudent : Operation<SchoolRegistry>
{
    public override Specification Define(SchoolRegistry registry) =>
        Named("Register Student")
            .When<StudentInfo>(students => students.Count <= 100)
            .With(
                from name in Fuzzr.String().Unique("student-name")
                from age in Fuzzr.Int()
                select (name, age))
            .Perform(input => registry.RegisterStudent(input.name, input.age))
            .Store(input => new StudentInfo(input.name, input.age, []))
            .Load(info => registry.FindStudent(info.Name))
            .FailsWith<ArgumentException>("Duplicate Name", input => (input.name, 42))
            .FailsWith<ArgumentException>("Empty string", input => (string.Empty, input.age))
            .FailsWith<ArgumentException>("Null string", input => (null!, input.age))
            .FailsWith<ArgumentException>("Whitespace string", input => (" ", input.age))
            .FailsWith<ArgumentException>("Age <= 0", input => (input.name, 0))
            .Expect(
                ("Name", (info, student) => info.Name == student.Name),
                ("Age", (info, student) => info.Age == student.Age));
}



