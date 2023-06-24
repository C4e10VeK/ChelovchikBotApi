using ChelovchikBotApi.Models.Repository;

namespace ChelovchikBotApi.Repositories;

public interface IUserRepository
{
    IEnumerable<WebApiUser> GetApiUsers();
}