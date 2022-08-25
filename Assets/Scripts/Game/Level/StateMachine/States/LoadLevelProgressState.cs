using Zenject;

namespace Game.Level.StateMachine.States
{
    public class LoadLevelProgressState : ILevelState
    {
        private readonly LevelStateMachine _levelStateMachine;

        public LoadLevelProgressState(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;
        }

        public void Enter()
        {
            _levelStateMachine.Enter<InitializeLevelState>();
        }

        public void Exit()
        {

        }
    }
}