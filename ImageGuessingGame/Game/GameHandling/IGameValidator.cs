using System.Collections.Generic;

namespace ImageGuessingGame.GameContext.GameHandling
{
    public interface IGameValidator
    {
        string[] IsValid(Game game);
    }
}