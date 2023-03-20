namespace API.UnitOfWorks;

public interface IUnitOfWork
{
    Task CompleteAsync();
}