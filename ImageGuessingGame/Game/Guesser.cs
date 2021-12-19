using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class Guesser
    {
        public Guesser(){}
        public Guesser(LoginUser user)
        {
            User = user;
        }
        public int Id{get;set;}
        public LoginUser User { get; set; }
    }
}