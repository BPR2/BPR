using BPR_RazorLibrary.Models;
using BPR_WebAPI.Models.Encryption;
using BPR_WebAPI.Persistence.Users;

namespace BPR_WebAPI.Services.Users;

public class UserService : IUserService
{
	IUserRepo accountRepo;
	public UserService(IConfiguration configuration)
	{
		accountRepo = new UserRepo(configuration);
	}

	public async Task<WebContent> ValidateAccount(User account)
	{
		account.Password = Encrypt.EncryptString(account.Password);

		var result = await GetAccountAsync(account.Username);

		User verifiedAccount = (User)result.content;

		if (verifiedAccount == null) return result; //webresponse should already have its state described

		if (verifiedAccount.Password == null) return new WebContent(WebResponse.ContentDataCorrupted, null);

		if (verifiedAccount.Password == account.Password) return new WebContent(WebResponse.AuthenticationSuccess, verifiedAccount);
		
		return new WebContent(WebResponse.AuthenticationFailure, null);
	}

	public async Task<WebContent> GetAccountAsync(string username)
	{
		var result = await accountRepo.GetUserAsync(username);

		if (result.response != WebResponse.ContentRetrievalSuccess) return result;

		return result;
	}

	public async Task<WebContent> GetAccountAsync(int id)
	{
		var result = await accountRepo.GetUserAsync(id);

		return result;
	}

	public async Task<WebResponse> CreateAccountAsync(User account)
	{
		account.Password = Encrypt.EncryptString(account.Password);
		return await accountRepo.CreateUserAsync(account);
	}		

	public async Task<WebResponse> UpdateAccountAsync(User account)
	{
		account.Password = Encrypt.EncryptString(account.Password);
		return await accountRepo.UpdateUserAsync(account);
	}

	public async Task<List<User>> GetAllAccountsAsync()
	{
		return await accountRepo.GetAllUsers();
	}
}
