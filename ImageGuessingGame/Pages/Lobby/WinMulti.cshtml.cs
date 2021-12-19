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
    public class WinMultiModel : PageModel
    {

        private readonly ILoginUserProvider _loginUserProvider;
        private readonly GuessingGameContext _guessingGameContext;
        private readonly IGameProvider _gameProvider;

        public string Winner;
        public int Score{get;set;}
        public string Label;
        public string LoggedUser;

        public WinMultiModel(ILoginUserProvider loginProvider, IGameProvider gameProvider
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
            Winner = game.Guesser.User.UserName;
            Score = game.Score;
            Label = game.Oracle.Label;
            await _gameProvider.UpdateGame(game); 
            return Page();
        }
    }

}