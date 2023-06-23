namespace ChelovchikBotApi.Models;

public class MongoDBConfig
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }
    public string SmileTable { get; set; } 
    public string UserTable { get; set; }
    public string ApiUserTable { get; set; }
}