using System.Collections.Generic;

namespace ImageGuessingGame
{
    public interface ILoginUserValidator
    {
        string[] IsValid(LoginUser loginUser);
    }
}