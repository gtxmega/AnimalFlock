using Data.Types;
using Events;
using Services.Currency;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.GameModes.Overrides
{
    public class MatchThreeGameMode : GameMode
    {
        public event Action<int> ChangeCurrentLossesActors;

        public int MaxActorsCount => _maxActorsCount;
        public int MaxChainLengthActors => _maxChainLengthActors;
        public int CurrencyPerNode => _currencyPerNode;

        public int MaxLostActors => _maxLostActors;
        public int CurrentLossesActors => _currentLostActors;
        
        [SerializeField] private int _maxActorsCount;
        [SerializeField] private int _maxChainLengthActors;
        [SerializeField] private int _currencyPerNode;

        [Space]
        [SerializeField] private int _maxLostActors;
        [SerializeField] private int _currentLostActors;

        private bool _gameEnd;

        private ActorGraph _actorGraph;

        //Services
        private IGameEventsListener _gameEventsListener;
        private IGameEventsExecuter _gameEventsExecuter;
        private ICurrencyVault _currencyVault;

        [Inject]
        private void Construct(IGameEventsListener gameEventsListener, IGameEventsExecuter gameEventsExecuter,
            ICurrencyVault currencyVault)
        {
            _gameEventsListener = gameEventsListener;
            _gameEventsExecuter = gameEventsExecuter;
            _currencyVault = currencyVault;
        }

        public override void ConstructMethod()
        {
            _actorGraph = new ActorGraph(_maxActorsCount);

            _gameEventsListener.ActorSpawned += OnActorSpawned;
            _gameEventsListener.ActorCollisionActor += OnActorCollisionActor;
            _gameEventsListener.ActorCollisionExitActor += OnActorCollisionExitActor;
            _gameEventsListener.ActorLeftGround += OnActorLeftGround;
        }

        private void OnActorLeftGround(Actor who)
        {
            _actorGraph.ClearNode(who.NodeIndex, actorCleaning: true);
            
            _currentLostActors++;

            ChangeCurrentLossesActors?.Invoke(_currentLostActors);

            if (_gameEnd) return;

            if (_currentLostActors >= _maxLostActors)
            {
                _gameEventsExecuter.OnLevelLose();
                _gameEnd = true;
            }
        }

        private void OnActorCollisionActor(Actor who, Actor target)
        {
            if(_actorGraph.TryActorConnect(who.NodeIndex, target.NodeIndex))
            {
                if(_actorGraph.GetConnectedCount(who.NodeIndex) > 0)
                {
                    List<int> nodes = new List<int>();

                    int chainLength = _actorGraph.GetChainLegthByActor(who.NodeIndex, who.NodeIndex, ref nodes);

                    if(chainLength >= MaxChainLengthActors)
                    {
                        DestroyChainOfNodes(ref nodes);
                        RewardForChainNodes(chainLength);
                    }
                }
            }
        }

        private void OnActorCollisionExitActor(Actor who, Actor target)
        {
            _actorGraph.DisconnectNodes(who.NodeIndex, target.NodeIndex);
        }

        private void RewardForChainNodes(int chainLength)
        {
            _currencyVault.ChangeCurrency(chainLength * _currencyPerNode);
        }

        private void DestroyChainOfNodes(ref List<int> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Actor actor = _actorGraph.TryGetActorByNodeIndex(nodes[i]);

                if (actor != null)
                {
                    actor.OnDead();
                }

                _actorGraph.ClearNode(nodes[i], actorCleaning: true);
            }
        }

        private void OnActorSpawned(Actor actor)
        {
            _actorGraph.TryAddActorToGraph(actor);
        }


        private void OnDestroy()
        {
            if(_gameEventsListener != null)
            {
                _gameEventsListener.ActorSpawned -= OnActorSpawned;
                _gameEventsListener.ActorCollisionActor -= OnActorCollisionActor;
                _gameEventsListener.ActorCollisionExitActor -= OnActorCollisionExitActor;
                _gameEventsListener.ActorLeftGround -= OnActorLeftGround;
            }
        }
    }
}
