using Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed.Common;

public static class UsersSeed
{
    public static void Seed(ModelBuilder builder)
    {
        var users = new[]
        {
            new AppUser
            {
                Id = "9b26f7f4-9f26-4229-971c-2baff8800f56",
                UserName = "test",
                NormalizedUserName = "TEST",
                Email = "test@test.com",
                NormalizedEmail = "TEST@TEST.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAIAAYagAAAAEJ7+rzibnKFaof1e/Gw8inugMFVXrorukNnhwqqQ2QrTOy8UAn7B5s0c1DEgImxsaw==",
                SecurityStamp = "PZP74NIMMZDGPNAGYAZJGT23WVSKE45C",
                ConcurrencyStamp = "5158fbfd-13b0-49d0-b855-1b344bb80186",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            }
        };

        builder.Entity<AppUser>().HasData(users);
    }
}