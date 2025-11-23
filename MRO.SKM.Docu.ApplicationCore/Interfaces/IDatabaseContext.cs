using Microsoft.EntityFrameworkCore;
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.ApplicationCore.Interfaces;

public interface IDatabaseContext
{
     DbSet<Repository> Repositories { get; set; }
     DbSet<RepositoryLanguage> RepositoryLanguages { get; set; }
     int SaveChanges();    

}