using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Infrastructure;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<RepositoryLanguage> RepositoryLanguages { get; set; }
    public DbSet<CodeFile> CodeFiles { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Method> Methods { get; set; }
    public DbSet<RepositoryAiConfiguration> RepositoryAiConfigurations { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return base.Database.BeginTransaction();
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await base.Database.BeginTransactionAsync();
    }

    public void CommitTransaction()
    {
        base.Database.CommitTransaction();
    }
    public async Task CommitTransactionAsync()
    {
        await base.Database.CommitTransactionAsync();
    }
}