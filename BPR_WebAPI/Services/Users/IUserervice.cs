using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Users;

public interface IUserService
{
    Task<WebContent> ValidateUser(User user);
    Task<WebContent> GetUserAsync(string username);
    Task<WebContent> GetUserAsync(int id);
    Task<WebResponse> CreateUserAsync(User user);
    Task<WebResponse> UpdateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
}
