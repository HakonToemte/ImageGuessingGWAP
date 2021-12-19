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
using ImageGuessingGame.GameContext.SuggestionHandling;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageGuessingGame.Pages
{
    public class SuggestionsModel : PageModel
    {
        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        private readonly ISuggestionProvider _suggestionProvider;
        public string LoggedUser;
        public SuggestionsModel(ILoginUserProvider loginProvider, IGameProvider gameProvider
                            , ISuggestionProvider suggestionProvider)
        {
            _loginUserProvider = loginProvider;
            _gameProvider = gameProvider;
            _suggestionProvider = suggestionProvider;
        }

        public async Task<ActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            var guid = HttpContext.Session.GetString("GameGuid");
            var suggestion = await _suggestionProvider.GetSuggestionByGuid(Guid.Parse(guid));
            var int_list = suggestion.IndexList.Select(x => x.index).ToArray();
            var game = await _gameProvider.GetGameByGuid(Guid.Parse(guid));
            var imageProcessor = new ImageProcessor();
            imageProcessor.ShowPartialImage(game.Oracle.ImagePath, int_list);
            return Page();
        }
    }

}