using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Users;

public interface IUserService
{
	Task<WebContent> ValidateAccount(User account);
	Task<WebContent> GetAccountAsync(string username);
	Task<WebContent> GetAccountAsync(int id);
	Task<WebResponse> CreateAccountAsync(User account);
	Task<WebResponse> UpdateAccountAsync(User account);
	Task<List<User>> GetAllAccountsAsync();
}
