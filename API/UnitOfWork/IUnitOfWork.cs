namespace API.UnitOfWork;

public interface IUnitOfWork
{
    Task CompleteAsync();
}