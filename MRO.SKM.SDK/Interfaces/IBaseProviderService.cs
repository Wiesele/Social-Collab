using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface IBaseProviderService
{
    Guid UUID { get; }
    string DisplayName { get; }
}