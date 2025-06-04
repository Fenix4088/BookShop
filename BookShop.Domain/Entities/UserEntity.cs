using System;

namespace BookShop.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }  

    private UserEntity() { }

    public static UserEntity Create(string email, string passwordHash, string role)
    {
        return new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Role = role
        };
    }

    public void ChangePassword(string newHash) => PasswordHash = newHash;
}