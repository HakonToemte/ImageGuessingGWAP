using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.Net.Cache;
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
    public class JoinMultiplayerModel : PageModel
    {

        private readonly ILoginUserProvider _loginUserProvider;
        private readonly IGameProvider _gameProvider;
        public LoginUser LoggedUser;
        public bool IsCreator = false;
        public Game Game;


        public JoinMultiplayerModel(ILoginUserProvider loginProvider, IGameProvider gameProvider)
        {
            _loginUserProvider = loginProvider;
            _gameProvider = gameProvider;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                var userName = HttpContext.Session.GetString("_Name");
                LoggedUser = await _loginUserProvider.GetLoginUserByName(userName);
                Guid gameId;
                try
                {
                    gameId = Guid.Parse(Request.Query["gameId"]);
                }
                catch
                {
                    gameId = Guid.Parse(HttpContext.Session.GetString("GameGuid"));
                }
                Game = await _gameProvider.GetGameByGuid(gameId);
                if (Game == null)
                {
                    return RedirectToPage("../Index");
                }

                if (!Game.MultiplayerOpenLobby)
                {
                    return RedirectToPage("../Index");
                }

                if (Game.Guesser.User == LoggedUser)
                {
                    IsCreator = true;
                }else{
                    HttpContext.Session.SetString("GameGuid", gameId.ToString());
                }

                return Page();
            }
            return RedirectToPage("../Index");
        }
    }

}