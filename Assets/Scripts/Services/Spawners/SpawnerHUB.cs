using UnityEngine;

namespace Services.Spawners
{
    public class SpawnerHUB : MonoBehaviour, ISpawnerHUB
    {
        [SerializeField] private ActorSpawnerBase[] _spawners;

        public void StartSpawners()
        {
            for (int i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].StartSpawn();
            }
        }

        public void StopSpawners()
        {
            for (int i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].StopSpawn();
            }
        }


    }
}