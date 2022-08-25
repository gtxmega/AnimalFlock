using System;

namespace Services.RandomizerService
{
    public class Randomizer : IRandomizer
    {
        private Random _randomizer;

        private int _currentRandomInt;

        public Randomizer()
        {
            _randomizer = new Random();
        }

        public int GetRandomRangeInt(int min, int max)
        {
            int random = UnityEngine.Random.Range(min, max);
            if(_currentRandomInt == random)
            {
                random = UnityEngine.Random.Range(min, max);
                _currentRandomInt = random;
            }

            return random;
        }

        public float GetRandomRangeFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}