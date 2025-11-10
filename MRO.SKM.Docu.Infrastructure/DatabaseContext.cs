using Microsoft.EntityFrameworkCore;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SMK.Docu.ApplicationCore.Models;

namespace MRO.SKM.Docu.Infrastructure;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    public DbSet<Repository> Repositories { get; set; }
}