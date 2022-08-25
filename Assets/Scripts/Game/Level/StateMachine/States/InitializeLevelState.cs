using Events;
using Game.GameModes;
using Zenject;

namespace Game.Level.StateMachine.States
{
    public class InitializeLevelState : ILevelState
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly IGameEventsExecuter _gameEventsExecuter;
        private readonly GameMode _gameMode;

        public InitializeLevelState(LevelStateMachine levelStateMachine, DiContainer diContainer, GameMode gameMode)
        {
            _levelStateMachine = levelStateMachine;
            _gameEventsExecuter = diContainer.Resolve<IGameEventsExecuter>();
            _gameMode = gameMode;
        }


        public void Enter()
        {
            _gameMode.ConstructMethod();
            _gameMode.BeginPlay();

            _gameEventsExecuter.OnLevelInitialized();
            _gameEventsExecuter.OnLevelReady();

            _levelStateMachine.Enter<LevelLoopState>();
        }

        public void Exit()
        {

        }
    }
}