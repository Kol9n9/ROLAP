namespace ROLAP.Utils
{
    public static class Utils
    {
        static readonly Random rand = new Random();
        public static double GetRandomDoubleInRange(double min, double max)
        {
            return (rand.NextDouble() * (max - min) + min);
        }
    }
}