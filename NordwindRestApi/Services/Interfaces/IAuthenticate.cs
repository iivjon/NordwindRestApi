using NordwindRestApi.Models;

namespace NordwindRestApi.Services.Interfaces
{
    public interface IAuthenticate
    {
        LoggedUser Authenticate(string username, string password);
    }
}
