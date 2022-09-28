using BPR_RazorLibrary.Models;
using System.Text;
using System.Text.Json;

namespace BPR_RazorLibrary.Services.Users
{
    public class UserService : IUserService
    {
#if DEBUG
        string url = "https://localhost:7109/api/Account";
#else
       
        string url = "";
#endif

        HttpClient client;

        private int userId;

        public UserService()
        {
            client = new HttpClient();
        }

        public async Task<User> ValidateUser(string username, string password)
        {
            string message = await client.GetStringAsync($"{url}/validate?username={username}&password={password}");
            try
            {
                User result = JsonSerializer.Deserialize<User>(message);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }

        }

        public async Task CreateAccount(User user)
        {
            string userSerialized = JsonSerializer.Serialize(user);

            HttpContent content = new StringContent(
                userSerialized,
                Encoding.UTF8,
                "application/json"
                );

            await client.PostAsync($"{url}/createAccount", content);
        }

        public async Task UpdateAccount(User user)
        {
            string userSerialized = JsonSerializer.Serialize(user);

            HttpContent content = new StringContent(
                userSerialized,
                Encoding.UTF8,
                "application/json"
                );

            await client.PutAsync($"{url}/updateAccount", content);
        }

        public int GetUserId()
        {
            return userId;
        }

        public void SetUserId(int id)
        {
            userId = id;
        }

        public async Task<List<User>> GetAllAccounts()
        {
            string message = await client.GetStringAsync($"{url}/allAccounts");
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
