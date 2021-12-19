using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ImageGuessingGame.GameContext;
using ImageGuessingGame.GameContext.GameHandling;

namespace ImageGuessingGame.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        [BindProperty]
        public IList<LoginUser> LoginUsers { get; set; }
        public LoginUser LoggedInUser;
        public List<Game> Games;
        public double AverageScore;
        public int GameCount;
        public string Error;
        public string LoggedUser;

        public ProfileModel(ILoginUserProvider loginUserProvider, IGameProvider gameProvider)
        {
            _loginUserProvider = loginUserProvider;
            _gameProvider = gameProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("_Name") == null)
            {
                Error = "You need to be logged in";
                return RedirectToPage("/Index");
            }
            LoggedUser = HttpContext.Session.GetString("_Name");
            
            LoggedInUser = await _loginUserProvider.GetLoginUserByName(LoggedUser);
            Games = await _gameProvider.GetGamesWithUser(LoggedInUser);
            GameCount = Games.Count();
            try
            {
                AverageScore = Games.Select(g => g.Score).Average();
            }
            catch (InvalidOperationException)
            {
                AverageScore = 0;
            }


            return Page();
        }
    }
}