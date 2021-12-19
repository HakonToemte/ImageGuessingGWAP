using System;
namespace ImageGuessingGame.GameContext
{
    public class MultiPlayerGuess
    {
        public MultiPlayerGuess(){}
        public MultiPlayerGuess(string user, string guess){
            LoginUserName = user;
            Guess = guess;
        }
        public int MultiPlayerGuessId{get;set;}
        public string LoginUserName{get;set;}
        public string Guess{get;set;}

        public Guid OracleId{get;set;}
        public Oracle Oracle{get;set;}

    }
}