using BPR_RazorLibrary.Models;
using Npgsql;

namespace BPR_WebAPI.Persistence.Users;

public class UserRepo : IUserRepo
{
    private readonly IConfiguration configuration;
    string connectionString;

    public UserRepo(IConfiguration iConfig)
    {
        configuration = iConfig;
        connectionString = configuration["ConnectionStrings:DefaultConnection"];
    }

    public async Task<WebResponse> CreateUserAsync(User user)
    {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) return WebResponse.ContentDataCorrupted;

        try
        {
            var isDuplicate = await IsUserAlreadyExist(user.Email, user.Username);

            if (isDuplicate) return WebResponse.ContentDuplicate;

            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"INSERT INTO public.Account(Username, Password, Name, Contact, Email, Location) VALUES (@Username, @Password, @Name, @Contact, @Email, @Location);";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Name", user.FullName);
                cmd.Parameters.AddWithValue("@Contact", user.Contact);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Location", user.Address);

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



    public async Task<WebContent> GetUserAsync(string username)
    {
        if (string.IsNullOrEmpty(username)) return new WebContent(WebResponse.ContentDataCorrupted, null);

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            WebContent result = new WebContent(WebResponse.Empty, null);
            string command = $"SELECT * FROM public.Account where Username = @Username ;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@Username", NpgsqlTypes.NpgsqlDbType.Varchar, username);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        result = ReadUser(reader);
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

    public async Task<WebContent> GetUserAsyncEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return new WebContent(WebResponse.ContentDataCorrupted, null);

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            WebContent result = new WebContent(WebResponse.Empty, null);
            string command = $"SELECT * FROM public.Account where Email = @Email ;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@Email", NpgsqlTypes.NpgsqlDbType.Varchar, email);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        result = ReadUser(reader);
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

    public async Task<WebContent> GetUserAsync(int id)
    {
        if (id < 0) return new WebContent(WebResponse.ContentDataCorrupted, null);

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"SELECT * FROM public.Account where accountid = @ID ;";
            WebContent result = new WebContent(WebResponse.Empty, null);
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        result = ReadUser(reader);
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

    public async Task<WebResponse> UpdateUserAsync(User user)
    {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) return WebResponse.ContentDataCorrupted;

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"UPDATE public.account\r\n\tSET password=@newPassword, contact=@newContact, email=@newEmail, location=@newLocation\r\n\tWHERE accountid = @accountId;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {   
                cmd.Parameters.AddWithValue("@newPassword", user.Password);
                cmd.Parameters.AddWithValue("@newContact", user.Contact);
                cmd.Parameters.AddWithValue("@newEmail", user.Email);
                cmd.Parameters.AddWithValue("@newLocation", user.Address);
                cmd.Parameters.AddWithValue("@accountId", user.AccountId);

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

    private async Task<bool> IsUserAlreadyExist(string desiredEmail, string desiredUsername)
    {
        var emailResult = await GetUserAsyncEmail(desiredEmail);

        if (emailResult.response == WebResponse.ContentRetrievalSuccess) return true;

        var usernameResult = await GetUserAsync(desiredUsername);

        if (usernameResult.response == WebResponse.ContentRetrievalSuccess) return true;

        return false;
    }

    private static WebContent ReadUser(NpgsqlDataReader reader)
    {
        try
        {
            User user = new User
            {
                AccountId = (int)(reader["AccountId"] as int?),
                Username = reader["Username"] as string,
                Password = reader["Password"] as string,
                FullName = reader["Name"] as string,
                Contact = reader["Contact"] as string,
                Email = reader["Email"] as string,
                Address = reader["Location"] as string,
            };
            return new WebContent(WebResponse.ContentRetrievalSuccess, user);
        }
        catch (Exception ex)
        {
            return new WebContent(WebResponse.ContentRetrievalFailure, null);
        }
    }

    public async Task<List<User>> GetAllUsers()
    {
        List<User> users = new List<User>();

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"SELECT * FROM public.Account where accountid > 1;";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User { 
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            AccountId = int.Parse(reader["accountid"].ToString()), 
                            FullName = reader["name"].ToString(),
                            Contact = reader["contact"].ToString(),
                            Email = reader["email"].ToString(),
                            Address = reader["location"].ToString()
                        });
                    }
            }
            con.Close();
            return users;
        }
        catch (Exception e)
        {
            throw new NotImplementedException();
        }
    }
}
