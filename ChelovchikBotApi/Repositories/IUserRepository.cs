using WebApiUser = ChelovchikBotApi.Models.Repository.WebApiUser;

namespace ChelovchikBotApi.Repositories;

public interface IUserRepository
{
    IEnumerable<WebApiUser> GetApiUsers();
}