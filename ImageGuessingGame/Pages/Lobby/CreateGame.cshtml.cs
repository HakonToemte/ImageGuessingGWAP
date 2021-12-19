using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ImageGuessingGame.GameContext.GameHandling;
using ImageGuessingGame.GameContext.SuggestionHandling;
using ImageGuessingGame.GameContext;

namespace ImageGuessingGame.Pages
{
    public class CreateGameModel : PageModel
    {
        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        private readonly ISuggestionProvider _suggestionProvider;
        [BindProperty]
        public ICollection<LoginUser> LoginUsers{get;set;}
        public string LoggedUser;
        public string Error;

        public CreateGameModel(ILoginUserProvider loginProvider,IGameProvider gameProvider
                                ,ISuggestionProvider suggestionProvider)
        {
            _loginUserProvider = loginProvider;
            _gameProvider = gameProvider;
            _suggestionProvider = suggestionProvider;
        }

        public ActionResult OnGet()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            return Page();
        }
        public async Task<ActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            else{
                Error="You need to be logged in";
                return Page();
            }
            var user = await _loginUserProvider.GetLoginUserByName(LoggedUser);
            var gamemode = Request.Form["gamemode"].ToString();
            var game = new Game(user, Int32.Parse(gamemode));
            await _gameProvider.AddGame(game);
            HttpContext.Session.SetString("GameGuid", game.Id.ToString());
            var suggestion = await _suggestionProvider.GetSuggestionForImage(game.Oracle.ImagePath);
            Console.WriteLine(suggestion!=null);
            if (suggestion != null)
            {
                game.Oracle.Suggestion = suggestion;
                await _gameProvider.UpdateGame(game);
            }
            if (Int32.Parse(gamemode) == 2)
            {
                return RedirectToPage("./JoinMultiplayer", new { userName = user.UserName });
            }
            
            return RedirectToPage("./Game");
        }
    }

}