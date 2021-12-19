using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class Game
    {
        public Game() { }
        public Game(LoginUser user, int gameMode = 0)
        {
            TimeStarted = DateTime.Now;
            // gameMode 0 = Single Player with Oracle
            // gameMode 1 = Single player with only uploaded images and oracle
            // gameMode 2 = Multi Player with multiple guessers and oracle.
            if (gameMode == 0)
            {
                Oracle = new Oracle();
                Oracle.Start();
                Guesser = new Guesser(user);
            }
            else if (gameMode == 1)
            {
                Oracle = new Oracle();
                Oracle.StartUpload();
                Guesser = new Guesser(user);
            }
            else if (gameMode == 2)
            {
                Oracle = new Oracle();
                Oracle.Start();
                Guesser = new Guesser(user);
                MultiplayerOpenLobby = true;
                GameMode = gameMode;

            }
        }
        public Guid Id { get; set; }
        public Oracle Oracle { get; set; }
        public Guesser Guesser { get; set; } // In multiplayer the Guesser is the Winner of the game
        public Gameplay Gameplay { get; set; }
        public List<Guesser> MultiplayerGuessers { get; set; }
        public bool MultiplayerOpenLobby { get; set; } = false;
        public int IncorrectCount { get; set; } = 0;
        public DateTime TimeStarted { get; set; }
        public int Score { get; set; } = 0;
        public bool Finished { get; set; } = false;
        public int GameMode { get; set; } = 0;

    }
}
