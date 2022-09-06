namespace BPR_WebAPI.Data.Accounts
{
	public interface IAccountService
	{
		Task<Models.Account> ValidateAccount(Models.Account account);
		Task<Models.Account> GetAccountAsync(string email);
		Task<Models.Account> GetAccountAsync(int id);
		Task CreateAccountAsync(Models.Account account);
		Task UpdateAccountAsync(Models.Account account);
	}
}
