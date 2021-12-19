using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class Suggestion
    {
        public Suggestion(){}
        public Suggestion(Guid id,string imagepath)
        {
            Id = id;
            ImagePath = imagepath;
            
        }
        public Guid Id{get;set;}
        public string ImagePath{get;set;}
        public List<IndexSuggestion> IndexList{get;set;}
    }
}