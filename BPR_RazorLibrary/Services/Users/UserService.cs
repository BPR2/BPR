using BPR_RazorLibrary.Models;
using System.Text;
using System.Text.Json;

namespace BPR_RazorLibrary.Services.Users
{
    public class UserService : IUserService
    {
#if DEBUG
        string url = "https://localhost:7109/api/User";
#else
       
        string url = "http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User";
#endif

        HttpClient client;

        private int? userId;

        public UserService()
        {
            client = new HttpClient();
        }

        public async Task<User> ValidateUser(string username, string password)
        {
            string message = await client.GetStringAsync($"{url}/validate?username={username}&password={password}");
            try
            {
                WebContent result = JsonSerializer.Deserialize<WebContent>(message);

                if(result.response != WebResponse.AuthenticationSuccess)
                {
                    return null;
                }

                var json = JsonSerializer.Serialize(result.content);

                var user = JsonSerializer.Deserialize<User>(json);

                SetUserId(user.AccountId);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }

        }

        public async Task CreateUser(User user)
        {
            string userSerialized = JsonSerializer.Serialize(user);

            HttpContent content = new StringContent(
                userSerialized,
                Encoding.UTF8,
                "application/json"
                );

            await client.PostAsync($"{url}/createUser", content);
        }

        public async Task UpdateUser(User user)
        {
            string userSerialized = JsonSerializer.Serialize(user);

            HttpContent content = new StringContent(
                userSerialized,
                Encoding.UTF8,
                "application/json"
                );

            await client.PutAsync($"{url}/updateUser", content);
        }

        public int? GetUserId()
        {
            return userId;
        }

        public void SetUserId(int? id)
        {
            userId = id;
        }

        public async Task<List<User>> GetAllUsers()
        {
            string message = await client.GetStringAsync($"{url}/allUsers");
            try
            {
                List<User> result = JsonSerializer.Deserialize<List<User>>(message);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
