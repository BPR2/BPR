using BPR_RazorLib.Models;
using BPR_WebAPI.Models;
using Npgsql;

namespace BPR_WebAPI.Persistence
{
	public class AccountRepo : IAccountRepo
	{
		private readonly IConfiguration configuration;
		string connectionString;

		public AccountRepo(IConfiguration iConfig)
		{
			configuration = iConfig;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}

		public async Task<WebResponse> CreateAccountAsync(Account account)
		{
			if (String.IsNullOrEmpty(account.Email) || String.IsNullOrEmpty(account.Password)) return WebResponse.ContentDataCorrupted;

			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				string command = $"INSERT INTO public.\"Account\"(\"Username\", \"Password\", \"Name\", \"Contact\", \"Email\", \"Location\") VALUES (@Username, @Password, @Name, @Contact, @Email, @Location);";
				await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
				{
					cmd.Parameters.AddWithValue("@Username", account.Username);
					cmd.Parameters.AddWithValue("@Password", account.Password);
					cmd.Parameters.AddWithValue("@Name", account.Name);
					cmd.Parameters.AddWithValue("@Contact", account.Contact);
					cmd.Parameters.AddWithValue("@Email", account.Email);
					cmd.Parameters.AddWithValue("@Location", account.Location);

					cmd.ExecuteNonQuery();
				}
				con.Close();
				return WebResponse.ContentCreateSuccess;
			}
			catch (Exception e)
			{
				return WebResponse.ContentCreateFailure;
			}
		}

		public async Task<WebContent> GetAccountAsync(string email)
		{
			if (String.IsNullOrEmpty(email)) return new WebContent(WebResponse.ContentDataCorrupted, null);

			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				WebContent result = new WebContent(WebResponse.Empty, null);
				string command = $"SELECT * FROM public.\"Account\" where \"Email\" = @Email ;";
				await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
				{
					cmd.Parameters.AddWithValue("@Email", NpgsqlTypes.NpgsqlDbType.Varchar, email);

					await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
						while (await reader.ReadAsync())
						{
							result = ReadAccount(reader);
							con.Close();
						}
				}
				con.Close();
				return result;
			}
			catch (Exception e)
			{
				return new WebContent(WebResponse.ContentRetrievalFailure, null);
			}
		}

		public async Task<WebContent> GetAccountAsync(int id)
		{
			if (id < 0) return new WebContent(WebResponse.ContentDataCorrupted, null);

			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				string command = $"SELECT * FROM public.\"Account\" where \"UserID\" = @ID ;";
				WebContent result = new WebContent(WebResponse.Empty, null);
				await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
				{
					cmd.Parameters.AddWithValue("@ID", id);

					await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
						while (await reader.ReadAsync())
						{
							result = ReadAccount(reader);
							con.Close();
						}
				}
				con.Close();
				return result;
			}
			catch (Exception e)
			{
				return new WebContent(WebResponse.ContentRetrievalFailure, null);
			}
		}

		public async Task<WebResponse> UpdateAccountAsync(Account account)
		{
			if (String.IsNullOrEmpty(account.Email) || String.IsNullOrEmpty(account.Password)) return WebResponse.ContentDataCorrupted;

			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				string command = $"INSERT INTO public.\"Account\"(\"Username\", \"Password\", \"Name\", \"Contact\", \"Email\", \"Location\") VALUES (@Username, @Password, @Name, @Contact, @Email, @Location);";
				await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
				{
					cmd.Parameters.AddWithValue("@Username", account.Username);
					cmd.Parameters.AddWithValue("@Password", account.Password);
					cmd.Parameters.AddWithValue("@Name", account.Name);
					cmd.Parameters.AddWithValue("@Contact", account.Contact);
					cmd.Parameters.AddWithValue("@Email", account.Email);
					cmd.Parameters.AddWithValue("@Location", account.Location);

					cmd.ExecuteNonQuery();
				}
				con.Close();
				return WebResponse.ContentUpdateSuccess;
			}
			catch (Exception e)
			{
				return WebResponse.ContentUpdateFailure;
			}
		}

		private static WebContent ReadAccount(NpgsqlDataReader reader)
		{
			try
			{
				Account account = new Account
				{
					AccountId = reader["AccountId"] as int?,
					Username = reader["Username"] as string,
					Password = reader["Password"] as string,
					Name = reader["Name"] as string,
					Contact = reader["Contact"] as string,
					Email = reader["Email"] as string,
					Location = reader["Location"] as string,
				};
				return new WebContent(WebResponse.ContentRetrievalSuccess, account);
			}
			catch (Exception ex)
			{
				return new WebContent(WebResponse.ContentRetrievalFailure, null);
			}
		}
	}
}
