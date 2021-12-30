using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class Game
    {
        public Game() { }
        // gameMode 0 = Single Player with Oracle
        // gameMode 1 = Single player with only uploaded images and oracle
        // gameMode 2 = Multi Player with multiple guessers and oracle.
        public Guid Id { get; set; }
        public Oracle Oracle { get; set; }
        public Guesser Guesser { get; set; } 
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
