using ChelovchikBotApi.Models;

namespace ChelovchikBotApi.Repository;

public interface IUserRepository
{
    Task<List<WebApiUser>> GetApiUsers();
}