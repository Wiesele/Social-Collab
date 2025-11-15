using Microsoft.EntityFrameworkCore;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.ApplicationCore.Interfaces;

public interface IDatabaseContext
{
     DbSet<Repository> Repositories { get; set; }
     int SaveChanges();
}