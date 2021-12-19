using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame
{
    public class LoginUser : IdentityUser // Inherits PasswordHash and UserName from IdentityUser
    {
        public int Highscore{get;set;}
    }
}