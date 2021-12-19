using System.Collections.Generic;
using System;

namespace ImageGuessingGame
{
    public class LoginUserValidator : ILoginUserValidator
    {
        public string[] IsValid(LoginUser loginUser){
            List<string> list= new List<string>();
            if (loginUser.UserName == null){
                list.Add("Name can not be empty");
            }
            if (loginUser.PasswordHash == null){
                list.Add("Password can not be empty");
            }
            string[] array = list.ToArray();
            return array;
        }
    }
}