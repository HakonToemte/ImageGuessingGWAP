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
    public class SuggestModel : PageModel
    {

        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        private readonly ISuggestionProvider _suggestionProvider;
        public string LoggedUser;
        public int Index;
        public SuggestModel(ILoginUserProvider loginProvider, IGameProvider gameProvider
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
            var game = await _gameProvider.GetGameByGuid(Guid.Parse(guid));
            var imageProcessor = new ImageProcessor();
            imageProcessor.ShowFullImage(game.Oracle.ImagePath);
            Index = -1;
            return Page();
        }
        public async Task<IActionResult> OnPostChooseAsync(string cursorx, string cursory){
            var guid = HttpContext.Session.GetString("GameGuid");
            var game = await _gameProvider.GetGameByGuid(Guid.Parse(guid));
            var xCor = Int32.Parse(cursorx);
            var yCor = Int32.Parse(cursory);
            var imagepath = game.Oracle.ImagePath;
            var imageProcessor = new ImageProcessor();
            int index_to_suggest  = imageProcessor.Find_slice_index(imagepath, xCor, yCor);
            GameContext.IndexSuggestion index = new GameContext.IndexSuggestion(index_to_suggest);
            if (await _suggestionProvider.GetSuggestionByGuid(Guid.Parse(guid)) == null) // First suggestion creates the list
            {
                var suggestion = new Suggestion(Guid.Parse(guid), imagepath);
                suggestion.IndexList = new List<GameContext.IndexSuggestion>();
                suggestion.IndexList.Add(index);
                await _suggestionProvider.AddSuggestions(suggestion);
            }
            else{ // adds more indexes to the list
                var suggestion = await _suggestionProvider.GetSuggestionByGuid(Guid.Parse(guid));
                suggestion.IndexList.Add(index);
                await _suggestionProvider.UpdateSuggestion(suggestion);
                if (suggestion.IndexList.Count >= 5){ // You only get to give 5 suggestions
                    return Redirect("Suggestions");
                }
            }
            Index=index.index;
            return Page();
        }
    }

}