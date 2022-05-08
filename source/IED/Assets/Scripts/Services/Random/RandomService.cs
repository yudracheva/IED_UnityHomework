namespace Services.Random
{
    public class RandomService : IRandomService
    {
        private readonly System.Random random;

        public RandomService()
        {
            random = new System.Random();
        }

        public int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        public int Next(int max)
        {
            return Next(0, max);
        }

        public float NextFloat()
        {
            return (float) NextDouble();
        }

        public double NextDouble()
        {
            return random.NextDouble();
        }
    }
}