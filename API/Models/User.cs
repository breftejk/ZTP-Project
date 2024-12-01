namespace API.Models;

public class User
{
    public string Id { get; set; }
    
    public User(string id)
    {
        Id = id;
    }
}