namespace QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary.Domain;

public class Student
{
    public string Name { get; }
    public int Age { get; }
    public Dictionary<string, int> scores = [];

    public Student(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");
        if (age <= 0)
            throw new ArgumentException("Invalid age");
        Name = name;
        Age = age;
    }

    public void AddOrUpdateScore(string subject, int score)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject is required");
        if (score < 0 || score > 100)
            throw new ArgumentException("Score must be between 0 & 100");
        scores[subject] = score;
    }

    public IReadOnlyDictionary<string, int> GetScores() => scores;

    public double GetAverage() => scores.Values.Average();

    public string GetScoreSOverview()
    {
        if (scores.Count == 0)
            return $"{Name} has no scores yet.";
        string result = $"{Name} scores:\n";
        foreach (var s in scores)
        {
            result += $"{s.Key}: {s.Value}\n";
        }
        return result;
    }
}