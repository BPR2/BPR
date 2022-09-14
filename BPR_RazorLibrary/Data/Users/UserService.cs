using BPR_RazorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPR_RazorLibrary.Data.Users
{
    public class UserService : IUserService
    {
        public Task CreateAccount(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByID(int id)
        {
            throw new NotImplementedException();
        }

        public int GetUserId()
        {
            throw new NotImplementedException();
        }

        public void SetUserId(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAccount(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ValidateUser(string email, string password)
        {
            User user = new User();
            user.Email = email;
            user.Password = password;
            user.UserID = 1;
            return user;                 

        }
    }
}
