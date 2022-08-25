
namespace Services.RandomizerService
{
    public interface IRandomizer
    {
        float GetRandomRangeFloat(float min, float max);
        int GetRandomRangeInt(int min, int max);
    }
}