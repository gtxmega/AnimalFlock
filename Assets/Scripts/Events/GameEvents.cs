using Data.Types;
using System;

namespace Events
{
    public class GameEvents : IGameEventsExecuter, IGameEventsListener
    {
        public event Action LevelBootstrap;

        public event Action LevelLoaded;
        public event Action LevelInitialized;
        public event Action LevelLoopEnter;
        public event Action LevelEnd;

        public event Action LevelWin;
        public event Action LevelLose;

        public event Action LevelReady;

        public event Action<Actor> ActorSpawned;
        public event Action<Actor, Actor> ActorCollisionActor;
        public event Action<Actor, Actor> ActorCollisionExitActor;
        public event Action<Actor> ActorLeftGround;
        public event Action<Actor> ActorDied;


        public void OnLevelBootstrap()
        {
            LevelBootstrap?.Invoke();
        }

        public void OnLevelLoaded()
        {
            LevelLoaded?.Invoke();
        }

        public void OnLevelInitialized()
        {
            LevelInitialized?.Invoke();
        }

        public void OnLevelLoopEnter()
        {
            LevelLoopEnter?.Invoke();
        }

        public void OnLevelReady()
        {
            LevelReady?.Invoke();
        }

        public void OnLevelEnd()
        {
            LevelEnd?.Invoke();
        }

        public void OnLevelWin()
        {
            LevelWin?.Invoke();
        }

        public void OnLevelLose()
        {
            LevelLose?.Invoke();
        }

        public void OnActorSpawned(Actor who)
        {
            ActorSpawned?.Invoke(who);
        }

        public void OnActorCollisionActor(Actor who, Actor target)
        {
            ActorCollisionActor?.Invoke(who, target);
        }

        public void OnActorCollisionExitActor(Actor who, Actor target)
        {
            ActorCollisionExitActor?.Invoke(who, target);
        }

        public void OnActorLeftGround(Actor actor)
        {
            ActorLeftGround?.Invoke(actor);
        }

        public void OnActorDied(Actor actor)
        {
            ActorDied?.Invoke(actor);
        }
    }
}