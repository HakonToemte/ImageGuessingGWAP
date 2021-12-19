using System;
namespace ImageGuessingGame.GameContext
{
    public class IndexSuggestion
    {
        public IndexSuggestion(){}
        public IndexSuggestion(int indx){
            index=indx;
        }
        public int IndexSuggestionId{get;set;}
        public int index{get;set;}

        public Guid SuggestionId{get;set;}
        public Suggestion Suggestion{get;set;}

    }
}