using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext.SuggestionHandling
{
    public class SuggestionProvider : ISuggestionProvider
    {
        private readonly GuessingGameContext _guessingGameContext;

        public SuggestionProvider(GuessingGameContext GuessingGameContext)
        {
            _guessingGameContext = GuessingGameContext;
        }

        public async Task<List<Suggestion>> GetSuggestions()
        {
            var list = _guessingGameContext.Suggestions.Where(s => s.Id == s.Id).Include(s => s.IndexList).ToList();
            return await Task.Run(() => list);
        }

        public Task AddSuggestions(Suggestion suggestion)
        {
            _guessingGameContext.Suggestions.Add(suggestion);
            _guessingGameContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<Suggestion> GetSuggestionByGuid(Guid id)
        {
            var suggestion = _guessingGameContext.Suggestions.Where(s => s.Id == id).
            Include(s => s.IndexList)
            .FirstOrDefault();
            if (suggestion == null)
            {
                return null;
            }
            return await Task.Run(() => suggestion);
        }
        public async Task<Suggestion> GetSuggestionForImage(string imagepath)
        {
            var suggestion = _guessingGameContext.Suggestions.Where(s => s.ImagePath == imagepath).
            Include(s => s.IndexList).FirstOrDefault();
            if (suggestion == null)
            {
                return null;
            }
            return await Task.Run(() => suggestion);
        }
        public Task UpdateSuggestion(Suggestion suggestion)
        {
            _guessingGameContext.Suggestions.Update(suggestion);
            _guessingGameContext.SaveChangesAsync();

            return Task.CompletedTask;
        }

        public Task RemoveSuggestion(int id)
        {
            _guessingGameContext.Suggestions.Remove(_guessingGameContext.Suggestions.Find(id));
            _guessingGameContext.SaveChangesAsync();
            return Task.CompletedTask;
        }


    }
}
