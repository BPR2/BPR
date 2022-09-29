using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence
{
	public interface IAccountRepo
	{
		Task<WebContent> GetAccountAsync(string username);
		Task<WebContent> GetAccountAsync(int id);
		Task<WebResponse> CreateAccountAsync(Account account);
		Task<WebResponse> UpdateAccountAsync(Account account);
        Task<List<Account>> GetAllAccounts();
    }
}
