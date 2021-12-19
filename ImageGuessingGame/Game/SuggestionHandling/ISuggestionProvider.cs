using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ImageGuessingGame.GameContext.SuggestionHandling
{
    public interface ISuggestionProvider
    {
        Task<List<Suggestion>> GetSuggestions();
        Task AddSuggestions(Suggestion suggestion);
        Task<Suggestion> GetSuggestionByGuid(Guid id);
        Task<Suggestion> GetSuggestionForImage(string imagepath);
        Task UpdateSuggestion(Suggestion Suggestion);
        Task RemoveSuggestion(int id);
    }
}
