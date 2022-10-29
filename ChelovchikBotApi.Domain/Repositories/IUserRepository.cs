using ChelovchikBotApi.Domain.Models.Repository;

namespace ChelovchikBotApi.Domain.Repositories;

public interface IUserRepository
{
    Task<List<WebApiUser>> GetApiUsers();
}