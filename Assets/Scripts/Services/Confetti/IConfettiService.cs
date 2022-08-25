using UnityEngine;

namespace Services.Confetti
{
    public interface IConfettiService
    {
        void PlayConfettiAt(Vector3 position, EConfettiColor confettiColor);
    }
}