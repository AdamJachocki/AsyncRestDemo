namespace AzureFunctions
{
    public class Randomizer
    {
        private readonly Random _random;

        public Randomizer()
        {
            _random = new Random();
        }

        public int GetInt(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
