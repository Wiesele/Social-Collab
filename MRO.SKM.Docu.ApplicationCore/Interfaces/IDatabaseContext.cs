using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.ApplicationCore.Interfaces;

public interface IDatabaseContext
{
    int SaveChanges();
    Task<int> SaveChangesAsync();
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
    void CommitTransaction();
    Task CommitTransactionAsync();
    
    DbSet<Repository> Repositories { get; set; }
    DbSet<RepositoryLanguage> RepositoryLanguages { get; set; }
    DbSet<RepositoryAiConfiguration> RepositoryAiConfigurations { get; set; }
    DbSet<CodeFile> CodeFiles { get; set; }
    DbSet<Class> Classes { get; set; }
    DbSet<Method> Methods { get; set; }
}