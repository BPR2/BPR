﻿using BPR_RazorLib.Models;
using BPR_WebAPI.Models;
using BPR_WebAPI.Models.Encryption;
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

		public async Task<WebContent> ValidateAccount(Account account)
		{
			account.Password = Encrypt.EncryptString(account.Password);

			var result = await GetAccountAsync(account.Email);

			Account verifiedAccount = (Account)result.content;

			if (verifiedAccount == null) return result; //webresponse should already have its state described

			if (verifiedAccount.Password == null) return new WebContent(WebResponse.ContentDataCorrupted, null);

			if (verifiedAccount.Password == account.Password) return new WebContent(WebResponse.AuthenticationSuccess, verifiedAccount);
			
			return new WebContent(WebResponse.AuthenticationFailure, null);
		}

		public async Task<WebContent> GetAccountAsync(string email)
		{
			var result = await accountRepo.GetAccountAsync(email);

			if (result.response != WebResponse.ContentRetrievalSuccess) return result;

			return result;
		}

		public async Task<WebContent> GetAccountAsync(int id)
		{
			var result = await accountRepo.GetAccountAsync(id);

			return result;
		}

		public async Task<WebResponse> CreateAccountAsync(Account account)
		{
			account.Password = Encrypt.EncryptString(account.Password);
			return await accountRepo.CreateAccountAsync(account);
		}		

		public async Task<WebResponse> UpdateAccountAsync(Account account)
		{
			account.Password = Encrypt.EncryptString(account.Password);
			return await accountRepo.UpdateAccountAsync(account);
		}		
	}
}