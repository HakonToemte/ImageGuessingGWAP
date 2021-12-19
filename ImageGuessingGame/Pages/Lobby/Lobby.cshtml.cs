using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ImageGuessingGame.GameContext.GameHandling;
using ImageGuessingGame.GameContext;



namespace ImageGuessingGame.Pages
{
    public class LobbyModel : PageModel
    {
        private readonly ILoginUserProvider _loginUserProvider;
        [BindProperty]
        public ICollection<LoginUser> LoginUsers { get; set; }
        private readonly IGameProvider _gameProvider;
        public string LoggedUser;
        public List<Game> Games { get; set; } = new();


        public LobbyModel(ILoginUserProvider Provider, IGameProvider gameProvider)
        {
            _loginUserProvider = Provider;
            _gameProvider = gameProvider;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            LoginUsers = await _loginUserProvider.GetLoginUsers();
            if (LoginUsers == null)
            {
                return NotFound();
            }
            var guid = HttpContext.Session.GetString("GameGuid");
            var game = await _gameProvider.GetGames();
            Games = game.Where(g=>g.Finished).OrderByDescending(g => g.TimeStarted).Take(5).ToList();
            return Page();
        }
    }
}