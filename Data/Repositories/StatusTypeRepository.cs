

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusTypeRepository
{
    private readonly IDbContextFactory<DataContext> _contextFactory;

    public StatusTypeRepository(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<StatusTypeEntity>> GetAllAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.StatusType.ToListAsync();
    }
}

