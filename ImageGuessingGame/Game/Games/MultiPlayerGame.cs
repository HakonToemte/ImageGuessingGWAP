using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class MultiPlayerGame : Game
    {
        public MultiPlayerGame() { }
        public MultiPlayerGame(LoginUser user)
        {
            Oracle = new Oracle();
            Oracle.Start();
            Guesser = new Guesser(user);
            MultiplayerOpenLobby = true;
            GameMode = 2;

        }
        public Guesser Winner{get;set;}

    }
}