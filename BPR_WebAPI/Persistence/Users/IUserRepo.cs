using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence.Users;

public interface IUserRepo
{
    Task<WebContent> GetUserAsync(string username);
    Task<WebContent> GetUserAsync(int id);
    Task<WebResponse> CreateUserAsync(User user);
    Task<WebResponse> UpdateUserAsync(User user);
    Task<List<User>> GetAllUsers();
}
