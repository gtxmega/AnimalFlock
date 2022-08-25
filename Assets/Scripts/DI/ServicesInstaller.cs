using Events;
using Game.GameModes;
using Services.Assets;
using Services.Confetti;
using Services.Currency;
using Services.Factory;
using Services.PoolObjects;
using Services.RandomizerService;
using UnityEngine;
using Zenject;

namespace DI
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private ConfettiService _confettiService;
        [SerializeField] private ActorsPool _actorsPool;
        [SerializeField] private GameMode _gameMode;

        private GameEvents _gameEventsInstance;
        private CurrencyVault _currencyVault;
        private AssetProvider _assets;
        private GameFactory _gameFactory;

        public override void InstallBindings()
        {
            _gameEventsInstance = new GameEvents();
            _currencyVault = new CurrencyVault();

            GameMode();

            AssetsProvider();
            GameFactory();
            GameEventsExecuter();
            GameEventsListener();
            Randomizer();

            ActorsPool();

            CurrencyVault();
            ConfettiServices();
        }

        private void GameMode()
        {
            Container
                .Bind<GameMode>()
                .To<GameMode>()
                .FromInstance(_gameMode)
                .AsSingle()
                .NonLazy();
        }

        private void ActorsPool()
        {
            Container
                .Bind<IActorsPool>()
                .To<ActorsPool>()
                .FromInstance(_actorsPool)
                .AsSingle()
                .NonLazy();
        }

        private void AssetsProvider()
        {
            Container
                .Bind<IAssets>()
                .To<AssetProvider>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void GameFactory()
        {
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void ConfettiServices()
        {
            Container
                .Bind<IConfettiService>()
                .To<ConfettiService>()
                .FromInstance(_confettiService)
                .AsSingle()
                .NonLazy();
        }

        private void CurrencyVault()
        {
            Container
                .Bind<ICurrencyVault>()
                .To<CurrencyVault>()
                .FromInstance(_currencyVault)
                .NonLazy();

            Container
                .Bind<ICurrencyVaultState>()
                .To<CurrencyVault>()
                .FromInstance(_currencyVault)
                .NonLazy();
        }

        private void GameEventsExecuter()
        {
            Container
               .Bind<IGameEventsExecuter>()
               .To<GameEvents>()
               .FromInstance(_gameEventsInstance)
               .NonLazy();
        }

        private void GameEventsListener()
        {
            Container
               .Bind<IGameEventsListener>()
               .To<GameEvents>()
               .FromInstance(_gameEventsInstance)
               .NonLazy();
        }

        private void Randomizer()
        {
            Container
                .Bind<IRandomizer>()
                .To<Randomizer>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}