using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ImageGuessingGame.GameContext
{
    public class Gameplay
    {
        public Gameplay(Guid gameId)
        {
            GameId = gameId;
        }
        [Key]
        public Guid GameId { get; set; }
        public void StartGame() { }
        public void NextTile() { }
        public bool CheckIfCorrect(string guess)
        {
            throw new NotImplementedException();
        }
    }
}