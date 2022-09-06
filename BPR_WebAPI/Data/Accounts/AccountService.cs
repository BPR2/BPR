using BPR_WebAPI.Models;
using BPR_WebAPI.Persistence;
using Microsoft.Extensions.Configuration;

namespace BPR_WebAPI.Data.Accounts
{
	public class AccountService : IAccountService
	{
		IAccountRepo accountRepo;
		public AccountService(IConfiguration configuration)
		{
			accountRepo = new AccountRepo(configuration);
		}

		public async Task<Account> ValidateAccount(Account account)
		{
			//TODO encryption?

			Models.Account verifiedAccount = await GetAccountAsync(account.Email);

			if (verifiedAccount == null) return null; //TODO possible return enum instead. would allow more specific fail reasons

			if (verifiedAccount.Password == null) return null;

			if (verifiedAccount.Password == account.Password) return verifiedAccount;
			
			return null;
		}

		public async Task<Account> GetAccountAsync(string email)
		{
			Models.Account account = await accountRepo.GetAccountAsync(email);

			if (account == null) return null;

			return account;
		}

		public async Task<Account> GetAccountAsync(int id)
		{
			return await accountRepo.GetAccountAsync(id);
		}

		public async Task CreateAccountAsync(Account account)
		{
			await accountRepo.CreateAccountAsync(account);
		}		

		public async Task UpdateAccountAsync(Account account)
		{
			await accountRepo.UpdateAccountAsync(account);
		}		
	}
}
