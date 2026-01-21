using InvestmentSimulator.Backend.Models.Domain;
using System.Collections.Concurrent;

namespace InvestmentSimulator.Backend.Services;

public class UserService : IUserService
{
    private readonly ConcurrentDictionary<string, User> _users = new();
    private readonly ConcurrentDictionary<string, string> _nameToIdMap = new();

    public User CreateOrGetUser(string name)
    {
        if (_nameToIdMap.TryGetValue(name, out var existingId)) 
        {
            return _users[existingId];
        }

        var user = new User
        {
            Name = name,
            Balance = 1000m,
        };

        _users.TryAdd(user.Id, user);
        _nameToIdMap.TryAdd(name, user.Id);

        return user;
    }

    public User? GetUser(string userId)
    {
        _users.TryGetValue(userId, out var user);
        return user;
    }

    public bool TryDeductBalance(string userId, decimal amount)
    {
        if(!_users.TryGetValue(userId, out var user))
            return false;

        lock (user)
        {
            if (user.Balance >= amount)
            {
                user.Balance -= amount;
                return true;
            }
            return false;
        }
    }

    public decimal AddBalance(string userId, decimal amount)
    {
        if(!_users.TryGetValue(userId,out var user))
            return 0;

        lock (user)
        {
            user.Balance += amount;
            return user.Balance;
        }
    }

}
