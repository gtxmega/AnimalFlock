using Data.Types;
using Events;
using Events.Entity;
using Services.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Services.PoolObjects
{
    public class ActorsPool : MonoBehaviour, IActorsPool
    {
        [SerializeField] private Actor[] _actors;
        [SerializeField] private int _poolSize;
        [SerializeField] private Transform _parentTransform;

        private Dictionary<EActorType, Queue<Actor>> _pool;
        private Dictionary<EActorType, Actor> _actorsPrefabStorage;

        //Services
        private IGameEventsListener _gameEventsListener;
        private IGameEventsExecuter _gameEventsExecuter;
        private IGameFactory _gamefactory;

        [Inject]
        private void Construct(IGameEventsListener gameEventsListener, IGameEventsExecuter gameEventsExecuter, IGameFactory gamefactory)
        {
            _gameEventsListener = gameEventsListener;
            _gameEventsExecuter = gameEventsExecuter;
            _gamefactory = gamefactory;


            _gameEventsListener.LevelBootstrap += OnLevelBootstrap;
            _gameEventsListener.ActorDied += OnActorDied;
        }


        private void OnLevelBootstrap()
        {
            FillPrefabStorage();

            _pool = new Dictionary<EActorType, Queue<Actor>>();

            CreatePool();

            _actorsPrefabStorage.Clear();

        }

        public Actor GetActorFromPool(EActorType actorType)
        {
            Actor actor = GetObjectFromPool(actorType);

            if(actor.IsDead)
                actor.EntityEvents.OnRespawn();
            else
                actor.gameObject.SetActive(true);

            _gameEventsExecuter.OnActorSpawned(actor);

            return actor;
        }

        private void OnActorDied(Actor actor)
        {
            _pool[actor.ActorType].Enqueue(actor);
        }

        private Actor GetObjectFromPool(EActorType actorType)
        {
            if (_pool[actorType].Count == 0)
            {
                AddNewObjectToPool(actorType);

                return GetObjectFromPool(actorType);
            }

            return _pool[actorType].Dequeue();
        }

        private void CreatePool()
        {
            foreach (KeyValuePair<EActorType, Actor> actor in _actorsPrefabStorage)
            {
                _pool.Add(actor.Key, new Queue<Actor>());

                for (int i = 0; i < _poolSize; i++)
                {
                    AddNewObjectToPool(actor.Key);
                }
            }
        }

        private void AddNewObjectToPool(EActorType actorType)
        {
            GameObject actorObject = _gamefactory
                .CreateGameObject(_actorsPrefabStorage[actorType].gameObject, _parentTransform);

            actorObject.gameObject.SetActive(false);

            Actor actor = actorObject.GetComponent<Actor>();

            if (actor.TryGetComponent<EntityEvents>(out EntityEvents entityEvents))
            {
                foreach (IEntityListener listener in actor.GetComponentsInChildren<IEntityListener>())
                {
                    listener.InitializeListener(entityEvents);
                }

                entityEvents.OnSpawned();
            }

            _pool[actorType].Enqueue(actor);
        }


        private void FillPrefabStorage()
        {
            _actorsPrefabStorage = new Dictionary<EActorType, Actor>();

            for (int i = 0; i < _actors.Length; ++i)
            {
                _actorsPrefabStorage.TryAdd(_actors[i].ActorType, _actors[i]);
            }
        }


        private void OnDestroy()
        {
            if (_gameEventsListener != null)
            {
                _gameEventsListener.LevelBootstrap -= OnLevelBootstrap;
                _gameEventsListener.ActorDied -= OnActorDied;
            }
        }
    }
}