
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext contex)
        {
            if(await contex.Users.AnyAsync())
                return;

            string userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            List<AppUser> users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                contex.Users.Add(user);
            }

            await contex.SaveChangesAsync();
        }
    }
}