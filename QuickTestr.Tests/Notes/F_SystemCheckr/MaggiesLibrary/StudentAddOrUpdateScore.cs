using QuickCheckr;
using QuickFuzzr;
using QuickPulse.Explains;
using QuickPulse.Instruments;
using QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;
using QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary.Domain;

namespace QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary;


// public record ScoreInfo(string? Subject, int Score);
[CodeExample]
public class StudentAddOrUpdateScore : Operation<SchoolRegistry>
{
    public override Specification Define(SchoolRegistry registry) =>
        Named("Student AddOrUpdateScore")
            .When<StudentInfo>(students => students.Count != 0)
            .With(
                from subject in Fuzzr.String()
                from score in Fuzzr.Int(1, 101)
                select (subject, score))
            .Perform((info, input) => registry.FindStudent(info.Name).AddOrUpdateScore(input.subject, input.score))
            .Store((info, input) => Chain.It(() => info.Scores[$"key-{input.subject}"] = input.score, info))
            .Load(info => registry.FindStudent(info.Name).scores)
            .FailsWith<ArgumentException>("Empty string", input => (string.Empty, input.score))
            .FailsWith<ArgumentException>("Null string", input => (null!, input.score))
            .FailsWith<ArgumentException>("Whitespace string", input => (" ", input.score))
            .FailsWith<ArgumentException>("Score < 0", input => (input.subject, -1))
            .FailsWith<ArgumentException>("Score > 100", input => (input.subject, 101))
            .Expect(("Scores", (info, scores) => scores.All(kv => kv.Value == info.Scores[$"key-{kv.Key}"])));
}