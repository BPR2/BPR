namespace BPR_WebAPI.Persistence
{
	public interface IAccountRepo
	{
		Task<Models.Account> GetAccountAsync(string email);
		Task<Models.Account> GetAccountAsync(int id);
		Task CreateAccountAsync(Models.Account account);
		Task UpdateAccountAsync(Models.Account account);
	}
}
