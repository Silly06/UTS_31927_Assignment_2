// File: PetPlayApp/PetPlayApp.Server/Services/Abstractions/IRepositoryProviderService.cs
using PetPlayApp.Server.Db;

namespace PetPlayApp.Server.Services.Abstractions
{
    public interface IRepositoryProviderService
    {
        IRepository<T> GetRepository<T>() where T : class;
    }
}
