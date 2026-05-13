namespace QuickTestr.Bolts.Modelr;

public sealed class WithModel<T>(string testName, string fileName, Func<T> model)
{
    private readonly Func<T> model = model;
    public WithSut<T, U> Sut<U>(Func<U> sut) => new(testName, fileName, model, sut);
}
