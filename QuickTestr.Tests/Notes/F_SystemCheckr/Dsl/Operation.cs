namespace QuickTestr.Tests.Notes.F_SystemCheckr.Dsl;


public abstract class Operation<TStashed>
{
    protected OperationBuilder Named(string name) => new(name);
    public abstract Specification Define(TStashed stashed);
}


