using BPR_RazorLibrary.Models;
using BPR_WebAPI.Models.Encryption;
using BPR_WebAPI.Persistence.Users;

namespace BPR_WebAPI.Services.Users;

public class UserService : IUserService
{
    IUserRepo userRepo;
    public UserService(IConfiguration configuration)
    {
        userRepo = new UserRepo(configuration);
    }

    public async Task<WebContent> ValidateUser(User user)
    {
        user.Password = Encrypt.EncryptString(user.Password);

        var result = await GetUserAsync(user.Username);

        User verifiedUser = (User)result.content;

        if (verifiedUser == null) return result; //webresponse should already have its state described

        if (verifiedUser.Password == null) return new WebContent(WebResponse.ContentDataCorrupted, null);

        if (verifiedUser.Password == user.Password) return new WebContent(WebResponse.AuthenticationSuccess, verifiedUser);

        return new WebContent(WebResponse.AuthenticationFailure, null);
    }

    public async Task<WebContent> GetUserAsync(string username)
    {
        var result = await userRepo.GetUserAsync(username);

        if (result.response != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }

    public async Task<WebContent> GetUserAsync(int id)
    {
        var result = await userRepo.GetUserAsync(id);

        return result;
    }

    public async Task<WebResponse> CreateUserAsync(User user)
    {
        user.Password = Encrypt.EncryptString(user.Password);
        return await userRepo.CreateUserAsync(user);
    }

    public async Task<WebResponse> UpdateUserAsync(User user)
    {
        if (!user.Password.Substring(user.Password.Length - 2).Equals("=="))
        {
            user.Password = Encrypt.EncryptString(user.Password);
        }
        return await userRepo.UpdateUserAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await userRepo.GetAllUsers();
    }
}
