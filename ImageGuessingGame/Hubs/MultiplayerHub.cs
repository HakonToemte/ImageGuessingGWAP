using System.IO.Pipes;
using System.Net.WebSockets;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ImageGuessingGame.GameContext;
using ImageGuessingGame.GameContext.GameHandling;

namespace ImageGuessingGame.Hubs
{
    public class MultiplayerHub : Hub
    {
        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        public MultiplayerHub(ILoginUserProvider loginUserProvider, IGameProvider gameProvider)
        {
            _loginUserProvider = loginUserProvider;
            _gameProvider = gameProvider;
        }

        public async Task SubmitGuess(string gameId, string guess, string userName)
        {
            var GameId = Guid.Parse(gameId);
            var game = await _gameProvider.GetGameByGuid(GameId);
            var games = await _gameProvider.GetGames();
            //var User = await _loginUserProvider.GetLoginUserByName(userName);
            if (game.Oracle.MultiplayerGuesses == null){
                game.Oracle.MultiplayerGuesses = new List<MultiPlayerGuess>();
            }
            Console.WriteLine(game.Oracle.Label);
            game.Oracle.MultiplayerGuesses.Add(new MultiPlayerGuess(userName, guess));
            if (game.Oracle.MultiplayerGuesses.Count == game.MultiplayerGuessers.Count) {
                var correctguess = false;
                foreach (var multiguess in game.Oracle.MultiplayerGuesses){
                    if (multiguess.Guess == game.Oracle.Label){
                        game.Guesser = new Guesser(await _loginUserProvider.GetLoginUserByName(multiguess.LoginUserName));
                        game.Oracle.ImageProcessor.ShowFullImage(game.Oracle.ImagePath);
                        game.Score = game.Oracle.PartialIndex.Count;
                        await _gameProvider.UpdateGame(game);
                        await Clients.All.SendAsync("Winner");
                        correctguess =true;
                    }
                }
                if (correctguess == false){
                    game.Oracle.SelectTile();
                    await Clients.All.SendAsync("WrongGuess");
                }
                game.Oracle.MultiplayerGuesses = new List<MultiPlayerGuess>();
            }
            await _gameProvider.UpdateGame(game);
        }

    }
}