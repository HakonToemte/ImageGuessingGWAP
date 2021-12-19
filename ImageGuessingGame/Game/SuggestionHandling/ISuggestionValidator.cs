using System.Collections.Generic;

namespace ImageGuessingGame.GameContext.SuggestionHandling
{
    public interface ISuggestionValidator
    {
        string[] IsValid(Suggestion suggestion);
    }
}