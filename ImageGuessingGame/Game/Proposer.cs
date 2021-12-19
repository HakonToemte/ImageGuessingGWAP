using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class Proposer : Oracle
    {
        public Proposer(LoginUser user)
        {
            User = user;
        }
        public LoginUser User { get; set; }
    }
}