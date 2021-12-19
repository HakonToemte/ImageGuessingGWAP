using System.Text.RegularExpressions;
using System;

namespace ImageGuessingGame.GameContext.GameHandling
{
    public class GuessValidator : IGuessValidator
    {
        public bool isCorrect(string label, string guess)
        {
            label = label.ToLower();
            label = Regex.Replace(label,"[^a-z]*","");
            guess = guess.ToLower();
            guess = Regex.Replace(guess,"[^a-z]*","");
            if (label == guess)
            {
                return true;
            }

            return false;
        }
    }
}