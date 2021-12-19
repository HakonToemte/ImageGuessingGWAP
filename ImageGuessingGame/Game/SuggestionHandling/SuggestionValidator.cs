using System.Collections.Generic;
using System;

namespace ImageGuessingGame.GameContext.SuggestionHandling
{
    public class SuggestionValidator : ISuggestionValidator
    {
        public string[] IsValid(Suggestion suggestion){
            List<string> list= new List<string>();
            if (suggestion.ImagePath == null){
                list.Add("ImagePath can not be empty");
            }
            string[] array = list.ToArray();
            return array;
        }
    }
}