using Core.Models;

using Microsoft.EntityFrameworkCore;

namespace AdminHotelApi.Repositories;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Hotel> Hotels { get; set; } = default!;
}
