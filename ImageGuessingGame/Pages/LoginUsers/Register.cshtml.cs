using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BC = BCrypt.Net.BCrypt;

namespace ImageGuessingGame.Pages.LoginUsers
{
    public class RegisterModel : PageModel
    {
        private readonly ILoginUserValidator _loginUserValidator;
        private readonly ILoginUserProvider _loginUserProvider;
        public RegisterModel(ILoginUserValidator Validator, ILoginUserProvider Provider)
        {
            _loginUserValidator = Validator;
            _loginUserProvider = Provider;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public LoginUser New_User {get; set; }
        public string Error {get;set;}

        public async Task<IActionResult> OnPostAsync()
        {
            string[] error_array = _loginUserValidator.IsValid(New_User);
            if (error_array.Length!=0)
            {
                Error = error_array[0];
                return Page();
            }
            New_User.PasswordHash = BC.HashPassword(New_User.PasswordHash);
            try
            {                                            // TRY CATCH FOR DUPLICATE NAMES
                await _loginUserProvider.AddLoginUser(New_User); // Successful register
                HttpContext.Session.SetString("_Name", New_User.UserName); 
                return RedirectToPage("../Index");
            }catch (Exception duplicate_error)
            {
                if (duplicate_error.InnerException.Message.Contains("UNIQUE constraint failed")){
                    Error = "NAME IS ALREADY USED";
                    return Page();
                }
                else {
                    Error = "ERROR";
                    return Page();
                }
            }
        }
    }

}
