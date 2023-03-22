using Infrastructure.Seed.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public static class SeedManager
{
    public static void Seed(ModelBuilder builder)
    {
        UsersSeed.Seed(builder);
    }
}