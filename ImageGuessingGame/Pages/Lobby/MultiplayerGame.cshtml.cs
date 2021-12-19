using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ImageGuessingGame.GameContext;
using ImageGuessingGame.GameContext.GameHandling;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageGuessingGame.Pages
{
    public class MultiplayerGameModel : PageModel
    {

        private readonly ILoginUserProvider _loginUserProvider;
        private readonly GuessingGameContext _guessingGameContext;
        private readonly IGameProvider _gameProvider;
        private readonly IGuessValidator _guessValidator;
        public ICollection<string> ImageSlices;
        public string LoggedUser;
        public Game Game { get; set; }


        public MultiplayerGameModel(ILoginUserProvider loginProvider, IGameProvider gameProvider
                            , GuessingGameContext guessingGameContext, IGuessValidator guessValidator)
        {
            _loginUserProvider = loginProvider;
            _gameProvider = gameProvider;
            _guessingGameContext = guessingGameContext;
            _guessValidator = guessValidator;
        }

        public async  Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            else
            {
                return RedirectToPage("../Index");
            }
            try
            {
                var gameId = Guid.Parse(Request.Query["gameId"]);
                Game = await _gameProvider.GetGameByGuid(gameId);
            }
            catch (Exception e)
            {
                var gameId = Guid.Parse(HttpContext.Session.GetString("GameGuid"));
                Game = await _gameProvider.GetGameByGuid(gameId);
            }
            return Page();
        }
    }

}