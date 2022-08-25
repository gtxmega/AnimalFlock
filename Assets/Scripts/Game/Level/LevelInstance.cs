using Game.Level.StateMachine;
using Game.Level.StateMachine.States;
using Events;
using Services.CoroutineService;
using UnityEngine;

using Zenject;
using Game.GameModes;

namespace Game.Level
{
    public class LevelInstance : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameMode _gameMode;

        private LevelStateMachine _levelStateMachine;

        private DiContainer _diContainer;
        private IGameEventsExecuter _gameEventsExecuter;


        [Inject]
        private void Construct(DiContainer diContainer, IGameEventsExecuter gameEventsExecuter)
        {
            _diContainer = diContainer;
            _gameEventsExecuter = gameEventsExecuter;
        }


        private void Awake()
        {
            _gameEventsExecuter.OnLevelBootstrap();

            _levelStateMachine = new LevelStateMachine(this, _diContainer, _gameMode);

            _levelStateMachine.Enter<LoadLevelState>();
        }

        private void OnDestroy()
        {
            _levelStateMachine.Enter<LevelDestroyState>();
        }
    }
}