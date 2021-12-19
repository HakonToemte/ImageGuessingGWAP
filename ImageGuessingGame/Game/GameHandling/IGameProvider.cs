using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ImageGuessingGame.GameContext.GameHandling
{
    public interface IGameProvider
    {
        Task<List<Game>> GetGames();
        Task AddGame(Game game);
        Task<Game> GetGameByGuid(Guid guid);
        Task<List<Game>> GetGamesWithUser(LoginUser user);
        Task UpdateGame(Game game);
        Task RemoveGame(int id);
    }
}
