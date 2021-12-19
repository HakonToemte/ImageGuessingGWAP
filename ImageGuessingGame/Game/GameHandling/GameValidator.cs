using System.Collections.Generic;
using System;

namespace ImageGuessingGame.GameContext.GameHandling
{
    public class GameValidator : IGameValidator
    {
        public string[] IsValid(Game game){
            List<string> list= new List<string>();
            if (game.Guesser.User == null){
                list.Add("User can not be empty");
            }
            string[] array = list.ToArray();
            return array;
        }
    }
}