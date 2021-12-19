using System.IO.Pipes;
using System.Net.WebSockets;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using ImageGuessingGame.GameContext;
using ImageGuessingGame.GameContext.GameHandling;

namespace ImageGuessingGame.Hubs
{
    public class StartGameHub : Hub
    {
        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        public StartGameHub(ILoginUserProvider loginUserProvider, IGameProvider gameProvider)
        {
            _loginUserProvider = loginUserProvider;
            _gameProvider = gameProvider;
        }
        
        public async Task StartGame(string gameId, string[] playernames)
        {
            var gameGuid = Guid.Parse(gameId);
            var game = await _gameProvider.GetGameByGuid(gameGuid);
            var games = await _gameProvider.GetGames();
            foreach (var playername in playernames)
            {
                var player = await _loginUserProvider.GetLoginUserByName(playername);
                var guesser = new Guesser(player);
                game.MultiplayerGuessers.Add(guesser);
            }
            await _gameProvider.UpdateGame(game);
            var game2 = await _gameProvider.GetGameByGuid(gameGuid);
            await Clients.All.SendAsync("ReceiveStart", gameId);
        }

        public async Task UpdateUsers()
        {
            await Clients.All.SendAsync("ClearList");
        }
        public async Task ProvideName(string userName)
        {
            await Clients.All.SendAsync("AddNameToList", userName);
        }
        public async Task HostLeft(string gameId)
        {
            var GameId = Guid.Parse(gameId);
            //await _gameProvider.RemoveGame(GameId);
            await UpdateUsers();
            await Clients.All.SendAsync("HostLeft");
        }
        public async Task PlayerLeft()
        {
            await UpdateUsers();
        }

    }
}