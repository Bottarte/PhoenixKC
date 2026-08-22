using Bogus;
using AwesomeAssertions;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.WebAPI.Features.Auth.Handlers.LoginUser;
using PhoenixKC.WebAPI.Features.Auth.Handlers.RegisterUser;

namespace PhoenixKC.IntegrationTests.Seeders;

public static class UserSeeder
{
    private static Faker Faker { get; } = new();

    public static async ValueTask<List<RegisterUserCommand>> AddUsersAsync(this AppFixture app, int min = 2, int max = 5)
    {
        List<RegisterUserCommand> users = [];
        for(int count = Faker.Random.Number(min, max); count > 0; count--)
        {
            RegisterUserCommand command = new Faker<RegisterUserCommand>().ValidInstance();
            using HttpResponseMessage message = await app.HttpClient.SendRegisterUserAsync(command, TestContext.Current.CancellationToken);
            message.EnsureSuccessStatusCode();
            users.Add(command);
        }
        return users;
    }
    public static async ValueTask<IEnumerable<(UserEntity User, string Password)>> AddUsers2Async(this AppFixture app, int min = 2, int max = 5)
    {
        List<RegisterUserCommand> commands = await app.AddUsersAsync(min, max);
        return await app.ExecuteDbContextAsync(async db =>
        {
            UserEntity[] users = await db.Users.AsNoTracking().Where(
                e => commands.Select(c => c.Email).Contains(e.Email)
            ).ToArrayAsync(TestContext.Current.CancellationToken);
            return users.Select(e =>
            {
                string password = commands.First(c => c.Email == e.Email).Password;
                return (e, password);
            });
        });
    }
    public static async ValueTask<RegisterUserCommand> AddUsersAndPickRandomAsync(this AppFixture app, int min = 2, int max = 5)
    {
        return Faker.PickRandom(await app.AddUsersAsync(min, max));
    }
    public static async ValueTask<(UserEntity User, string Password)> AddUsers2AndPickRandomAsync(this AppFixture app, int min = 2, int max = 5)
    {
        return Faker.PickRandom(await app.AddUsers2Async(min, max));
    }
    public static async ValueTask<(UserEntity User, string Password, string AccessToken)> AddUsers2AndLoginRandomAsync(this AppFixture app, int min = 2, int max = 5)
    {
        (UserEntity user, string password) = await app.AddUsers2AndPickRandomAsync(min, max);
        LoginUserCommand login = new Faker<LoginUserCommand>().ValidInstance().WithEmail(user.Email!).WithPassword(password);
        using HttpResponseMessage response = await app.HttpClient.SendLoginUserAsync(login, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        LoginUserResponse? result = await response.Content.ReadFromJsonAsync<LoginUserResponse>(TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        return (user, password, result.AccessToken);
    }
}