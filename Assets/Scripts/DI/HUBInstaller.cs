using Services.Spawners;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.DI
{
    public class HUBInstaller : MonoInstaller
    {
        [SerializeField] private SpawnerHUB _spawnerHUB;

        public override void InstallBindings()
        {
            Spawner();
        }

        private void Spawner()
        {
            Container
                .Bind<ISpawnerHUB>()
                .To<SpawnerHUB>()
                .FromInstance(_spawnerHUB)
                .AsSingle()
                .NonLazy();
        }
    }
}