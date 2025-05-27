using Microsoft.EntityFrameworkCore;
using TechCorpApi.Models;

namespace TechCorpApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
