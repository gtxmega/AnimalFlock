using Events;
using Services.Spawners;
using Zenject;

namespace Game.Level.StateMachine.States
{
    public class LevelLoopState : ILevelState
    {
        private readonly LevelStateMachine _levelStateMachine;

        private readonly ISpawnerHUB _spawnerHUB;
        private readonly IGameEventsListener _gameEventsListener;
        private readonly IGameEventsExecuter _gameEventsExecuter;

        public LevelLoopState(LevelStateMachine levelStateMachine, DiContainer diContainer)
        {
            _levelStateMachine = levelStateMachine;

            _spawnerHUB = diContainer.Resolve<ISpawnerHUB>();

            _gameEventsListener = diContainer.Resolve<IGameEventsListener>();
            _gameEventsExecuter = diContainer.Resolve<IGameEventsExecuter>();

            _gameEventsListener.LevelWin += OnLevelWin;
            _gameEventsListener.LevelLose += OnLevelLose;
        }

        public void Enter()
        {
            _gameEventsExecuter.OnLevelLoopEnter();

            _spawnerHUB.StartSpawners();
        }

        private void OnLevelWin()
        {
            _spawnerHUB.StopSpawners();
        }

        private void OnLevelLose()
        {
            _spawnerHUB.StopSpawners();
        }

        public void Exit()
        {
            _gameEventsListener.LevelWin -= OnLevelWin;
            _gameEventsListener.LevelLose -= OnLevelLose;
        }

    }
}