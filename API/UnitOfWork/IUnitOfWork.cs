using Infrastructure.Context;

namespace API.UnitOfWork;

public interface IUnitOfWork
{
    AppDbContext DbContext { get; }

    Task CompleteAsync();
}