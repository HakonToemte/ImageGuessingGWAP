using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext.GameHandling
{
    public class GameProvider : IGameProvider
    {
        private readonly GuessingGameContext _guessingGameContext;

        public GameProvider(GuessingGameContext GuessingGameContext)
        {
            _guessingGameContext = GuessingGameContext;
        }

        public async Task<List<Game>> GetGames()
        {
            var list = _guessingGameContext.Games.Where(u => u.Id == u.Id).Include(g => g.Guesser)
            .Include(g => g.Oracle).ToList();
            return await Task.Run(() => list);
        }

        public Task AddGame(Game game)
        {
            _guessingGameContext.Games.Add(game);
            _guessingGameContext.Oracles.Add(game.Oracle);
            _guessingGameContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<Game> GetGameByGuid(Guid guid)
        {
            var game = _guessingGameContext.Games.Where(g => g.Id == guid).
            Include(g => g.Oracle).Include(g => g.Oracle.PartialIndex)
            .Include(g=>g.Oracle.Suggestion).Include(g=>g.Oracle.Suggestion.IndexList).
            Include(g => g.Guesser).Include(g => g.MultiplayerGuessers).
            Include(g=>g.Oracle.MultiplayerGuesses).Include(g=>g.Guesser.User).FirstOrDefault();
            if (game == null)
            {
                return null;
            }
            return await Task.Run(() => game);
        }

        public async Task<List<Game>> GetGamesWithUser(LoginUser user)
        {
            var games = await _guessingGameContext.Games
                        .Where(g => g.Guesser.User == user && g.Finished)
                        .Include(g => g.Oracle)
                        .Include(g => g.Oracle.PartialIndex).ToListAsync();
            if (games == null)
            {
                return null;
            }
            return games;
        }

        public Task UpdateGame(Game game)
        {
            _guessingGameContext.Oracles.Update(game.Oracle);
            _guessingGameContext.SaveChangesAsync();

            return Task.CompletedTask;
        }

        public Task RemoveGame(int id)
        {
            _guessingGameContext.Games.Remove(_guessingGameContext.Games.Find(id));
            _guessingGameContext.SaveChangesAsync();
            return Task.CompletedTask;
        }


    }
}
