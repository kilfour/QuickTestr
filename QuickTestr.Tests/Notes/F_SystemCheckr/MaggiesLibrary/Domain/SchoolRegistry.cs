namespace QuickTestr.Tests.Notes.F_SystemCheckr.MaggiesLibrary.Domain;

public class SchoolRegistry
{
    private readonly List<Student> students = [];

    public void RegisterStudent(string name, int age)
    {
        if (students.Any(s => s.Name == name))
            throw new ArgumentException("Student bestaat al");
        students.Add(new Student(name, age));
    }

    public Student FindStudent(string name)
    {
        var student = students.FirstOrDefault(s => s.Name == name);
        if (student == null)
            throw new ArgumentException("Student bestaat niet");
        return student;
    }

    public IReadOnlyList<Student> GetAllStudents() => students;

    public IReadOnlyList<Student> GetTopStudents(int count) =>
        [.. students.OrderByDescending(s => s.GetAverage()).Take(count)];
}

