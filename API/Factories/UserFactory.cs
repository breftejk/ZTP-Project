using API.Data;
using API.Models;

namespace API.Factories;

/// <summary>
/// Factory for creating and managing users.
/// </summary>
public class UserFactory
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFactory"/> class.
    /// </summary>
    /// <param name="dbContext">The application's database context.</param>
    public UserFactory(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves an existing user or creates a new one if the user does not exist.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The user entity.</returns>
    public User GetOrCreateUser(string userId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            user = new User { Id = userId };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        return user;
    }
}