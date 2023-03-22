using AutoMapper;
using Infrastructure.Context;

namespace API.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext context;

    public UnitOfWork(AppDbContext context, IMapper mapper)
    {
        this.context = context;
    }

    public void Dispose() => context.Dispose();

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}