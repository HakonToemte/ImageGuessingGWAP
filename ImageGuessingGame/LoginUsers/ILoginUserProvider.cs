using System.Threading.Tasks;
using System.Collections.Generic;

namespace ImageGuessingGame
{
    public interface ILoginUserProvider
    {
        Task<IList<LoginUser>> GetLoginUsers();
        Task<IList<LoginUser>> GetLoginUsersOrdered();
        Task AddLoginUser(LoginUser loginUser);
        Task<LoginUser> GetLoginUserByName(string name);
        Task UpdateLoginUser(LoginUser loginUser);
        Task RemoveLoginUser(int id);
    }
}
