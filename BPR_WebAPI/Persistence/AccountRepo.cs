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

		public async Task CreateAccountAsync(Account account)
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
		}

		public async Task<Account> GetAccountAsync(string email)
		{
			using var con = new NpgsqlConnection(connectionString);
			con.Open();

			string command = $"SELECT * FROM public.\"Account\" where \"Email\" = @Email ;";
			await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
			{
				cmd.Parameters.AddWithValue("@Email", NpgsqlTypes.NpgsqlDbType.Varchar, email);

				await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
					while (await reader.ReadAsync())
					{
						Models.Account account = ReadAccount(reader);
						con.Close();
						return account;
					}
			}
			con.Close();
			return null;
		}

		public async Task<Account> GetAccountAsync(int id)
		{
			using var con = new NpgsqlConnection(connectionString);
			con.Open();

			string command = $"SELECT * FROM public.\"Account\" where \"UserID\" = @ID ;";
			await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
			{
				cmd.Parameters.AddWithValue("@ID", id);

				await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
					while (await reader.ReadAsync())
					{
						Models.Account account = ReadAccount(reader);
						con.Close();
						return account;
					}
			}
			con.Close();
			return null;
		}

		public async Task UpdateAccountAsync(Account account)
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
		}

		private static Models.Account ReadAccount(NpgsqlDataReader reader)
		{
			try
			{
				Models.Account account = new Models.Account
				{
					AccountId = reader["AccountId"] as int?,
					Username = reader["Username"] as string,
					Password = reader["Password"] as string,
					Name = reader["Name"] as string,
					Contact = reader["Contact"] as string,
					Email = reader["Email"] as string,
					Location = reader["Location"] as string,
				};
				return account;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
