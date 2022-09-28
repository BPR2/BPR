using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Accounts;

public interface IAccountService
{
	Task<WebContent> ValidateAccount(Account account);
	Task<WebContent> GetAccountAsync(string username);
	Task<WebContent> GetAccountAsync(int id);
	Task<WebResponse> CreateAccountAsync(Account account);
	Task<WebResponse> UpdateAccountAsync(Account account);
	Task<List<Account>> GetAllAccountsAsync();
}
