

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository 
{
    private readonly IDbContextFactory<DataContext> _contextFactory;

    public UserRepository(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    
    public async Task<UserEntity?> GetByIdAsync(int id)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<UserEntity>> GetAllAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.Users.ToListAsync();
    }

    public async Task CreateAsync(UserEntity user)
    {
        await using var context = _contextFactory.CreateDbContext();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        await using var context = _contextFactory.CreateDbContext();
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var entity = await context.Users.FindAsync(id);
        if (entity != null)
        {
            context.Users.Remove(entity);
            await context.SaveChangesAsync();
        }
    }


}