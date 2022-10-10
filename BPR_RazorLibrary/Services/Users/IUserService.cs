﻿using BPR_RazorLibrary.Models;

namespace BPR_RazorLibrary.Services.Users
{
    public interface IUserService
    {
        Task<User> ValidateUser(string username, string password);
        void SetUserId(int? id);
        int? GetUserId();
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task<List<User>> GetAllUsers();
    }
}
