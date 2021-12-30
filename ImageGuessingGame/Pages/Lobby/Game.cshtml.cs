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
    public class GameModel : PageModel
    {

        private readonly ILoginUserProvider _loginUserProvider;
        private readonly GuessingGameContext _guessingGameContext;
        private readonly IGameProvider _gameProvider;
        private readonly IGuessValidator _guessValidator;
        public string LoggedUser;
        public string Label;
        public string Guess;
        public string Success;


        public GameModel(ILoginUserProvider loginProvider, IGameProvider gameProvider
                            , GuessingGameContext guessingGameContext, IGuessValidator guessValidator)
        {
            _loginUserProvider = loginProvider;
            _gameProvider = gameProvider;
            _guessingGameContext = guessingGameContext;
            _guessValidator = guessValidator;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            var user = await _loginUserProvider.GetLoginUserByName(LoggedUser);
            var guid = HttpContext.Session.GetString("GameGuid");
            var game = await _gameProvider.GetGameByGuid(Guid.Parse(guid));
            var gameplay = new Gameplay(game.Id);
            Label = game.Oracle.Label;
            Guess = Request.Form["Guess"];
            Console.WriteLine(Label);
            if (Request.Form["Guess"] != "")
            {
                if (_guessValidator.isCorrect(Label, Guess))
                {
                    Success = "Success you won";
                    var score = game.Oracle.PartialIndex.Count;
                    if (user.Highscore == 0)
                    {
                        user.Highscore = score;
                    }
                    else
                    {
                        if (score < user.Highscore)
                        {
                            user.Highscore = score;
                        }
                    }
                    game.Finished = true;
                    game.Score = score;
                    await _gameProvider.UpdateGame(game);
                    await _loginUserProvider.UpdateLoginUser(user);
                    return RedirectToPage("./Win");

                }
                else
                {
                    game.IncorrectCount++;
                    if (game.IncorrectCount >= 3)
                    {
                        game.Oracle.SelectTile();
                        game.IncorrectCount = 0;
                    }
                    await _gameProvider.UpdateGame(game);
                }
            }
            else
            {
                game.Oracle.SelectTile();
                await _gameProvider.UpdateGame(game);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostQuitAsync()
        {
            var guid = HttpContext.Session.GetString("GameGuid");
            var game = await _gameProvider.GetGameByGuid(Guid.Parse(guid));
            game.Finished=true;
            return RedirectToPage("Lose");
        }
    }

}