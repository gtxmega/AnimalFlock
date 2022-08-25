

using Data.Types;

namespace Events
{
    public interface IGameEventsExecuter
    {
        void OnActorCollisionActor(Actor who, Actor target);
        void OnActorCollisionExitActor(Actor who, Actor target);
        void OnActorDied(Actor actor);
        void OnActorLeftGround(Actor actor);
        void OnActorSpawned(Actor who);
        void OnLevelBootstrap();
        void OnLevelEnd();
        void OnLevelInitialized();
        void OnLevelLoaded();
        void OnLevelLoopEnter();
        void OnLevelLose();
        void OnLevelReady();
        void OnLevelWin();
    }
}