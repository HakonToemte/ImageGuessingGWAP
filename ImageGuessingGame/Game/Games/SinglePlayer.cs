using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class SinglePlayerGame : Game
    {
        public SinglePlayerGame() { }
        public SinglePlayerGame(LoginUser user)
        {
            TimeStarted = DateTime.Now;
            Oracle = new Oracle();
            Oracle.Start();
            Guesser = new Guesser(user);
        }

    }
}
