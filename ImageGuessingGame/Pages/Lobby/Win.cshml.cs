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
    public class WinModel : PageModel
    {

        private readonly ILoginUserProvider _loginUserProvider;
        private readonly GuessingGameContext _guessingGameContext;
        private readonly IGameProvider _gameProvider;

        public string LoggedUser;
        public string Label{get;set;}
        public int Score{get;set;}

        public WinModel(ILoginUserProvider loginProvider, IGameProvider gameProvider
                            , GuessingGameContext guessingGameContext)
        {
            _loginUserProvider = loginProvider;
            _gameProvider = gameProvider;
            _guessingGameContext = guessingGameContext;
        }

        public async Task<ActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            var guid = HttpContext.Session.GetString("GameGuid");
            var game = await _gameProvider.GetGameByGuid(Guid.Parse(guid));
            game.Oracle.ImageProcessor.ShowFullImage(game.Oracle.ImagePath);
            Label = game.Oracle.Label;
            Score = game.Score;
            await _gameProvider.UpdateGame(game); 
            return Page();
        }
    }

}