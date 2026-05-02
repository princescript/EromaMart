using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Persistence;
using System.Linq.Expressions;

namespace Server.Repositories;

public interface IUserRepository
{
    Task RegisterUserAsync(UserMaster entity , CancellationToken ct = default);
    Task<UserMaster?> FindUserAsync(Expression<Func<UserMaster,bool>> predicate, CancellationToken ct = default);
}
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task RegisterUserAsync(UserMaster entity, CancellationToken ct = default)
    {
         await _context.DbUserMaster.AddAsync(entity, ct);
         await _context.SaveChangesAsync();

    }
    public async Task<UserMaster?> FindUserAsync(Expression<Func<UserMaster, bool>> predicate,CancellationToken ct = default)
    {
        return await _context.DbUserMaster
            .AsNoTracking()                 
            .FirstOrDefaultAsync(predicate, ct);
    }

}
