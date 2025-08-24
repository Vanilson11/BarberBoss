using Microsoft.EntityFrameworkCore;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Infrastructure.DataAccess;
internal class BarberBossDbContext : DbContext
{
    public BarberBossDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Revenue> Revenues { get; set; }
}
