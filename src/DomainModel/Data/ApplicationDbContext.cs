using Microsoft.EntityFrameworkCore;

namespace DomainModel.Data;

/// <summary>
/// Never use this class for database access - always use DomainModelTransaction
/// </summary>
public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions options) : base(options) {
    }
}