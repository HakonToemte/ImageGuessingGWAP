using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ImageGuessingGame.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILoginUserProvider _loginUserProvider;
        [BindProperty]
        public IList<LoginUser> LoginUsers{get;set;}
        public string LoggedUser;

        public IndexModel(ILoginUserProvider Provider)
        {
            _loginUserProvider = Provider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            var users = await _loginUserProvider.GetLoginUsersOrdered();
            var counter = 0;
            for (int i = 0; i < users.Count;i++){
                var item = users[counter];
                if (item.Highscore == 0){
                    users.RemoveAt(counter);
                    users.Add(item);
                    counter --;
                }
                counter ++;
            }
            LoginUsers=users;
            if (LoginUsers == null)
            {
                return NotFound();
            }
            return Page();
        }
    }

}