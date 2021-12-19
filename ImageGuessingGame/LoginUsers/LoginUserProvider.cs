using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame
{
    public class LoginUserProvider : ILoginUserProvider
    {
        private readonly GuessingGameContext _guessingGameContext;

        public LoginUserProvider(GuessingGameContext GuessingGameContext)
        {
            _guessingGameContext = GuessingGameContext;
        }

        public async Task<IList<LoginUser>> GetLoginUsers()
        {
            var list = _guessingGameContext.LoginUsers.Where(u => u.Id== u.Id).ToList();
            return await Task.Run(() => list);
        }
        public async Task<IList<LoginUser>> GetLoginUsersOrdered()
        {
            var list = _guessingGameContext.LoginUsers.Where(u => u.Id== u.Id).OrderBy(u => u.Highscore).ToList();
            return await Task.Run(() => list);
        }

        public Task AddLoginUser(LoginUser loginUser){
            _guessingGameContext.LoginUsers.Add(loginUser);
            _guessingGameContext.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<LoginUser> GetLoginUserByName(string name){
            var loginUser = _guessingGameContext.LoginUsers.Where(b => b.UserName == name).FirstOrDefault();
            if (loginUser == null)
            {
                return null;
            }
            return await Task.Run(() => loginUser);
        }

        public Task UpdateLoginUser(LoginUser loginUser)
        {
            _guessingGameContext.LoginUsers.Update(loginUser);
            _guessingGameContext.SaveChanges();
            
            return Task.CompletedTask;
        }

        public Task RemoveLoginUser(int id)
        {
            _guessingGameContext.LoginUsers.Remove(_guessingGameContext.LoginUsers.Find(id));
            _guessingGameContext.SaveChanges();
            return Task.CompletedTask;
        }

        
    }
}
