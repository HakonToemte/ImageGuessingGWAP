namespace ImageGuessingGame.GameContext.GameHandling
{
    public interface IGuessValidator
    {
        bool isCorrect(string label, string guess);
    }
}