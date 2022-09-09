using BPR_RazorLib.Models;
using BPR_WebAPI.Models;

namespace BPR_WebAPI.Persistence
{
	public interface IAccountRepo
	{
		Task<WebContent> GetAccountAsync(string email);
		Task<WebContent> GetAccountAsync(int id);
		Task<WebResponse> CreateAccountAsync(Account account);
		Task<WebResponse> UpdateAccountAsync(Account account);
	}
}
