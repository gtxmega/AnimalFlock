using Components;
using Data.Types;
using Events;
using Events.Entity;
using Services.Assets;
using Services.PoolObjects;
using Services.RandomizerService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Services.Spawners
{
    public class ActorSpawner : ActorSpawnerBase
    {
        private int _currentActorIndex;

        private IRandomizer _randomizer;


        [Inject]
        private void Construct(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public override void StartSpawn()
        {
            StartCoroutine(SpawnProcess());
        }

        public override void StopSpawn()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnProcess()
        {
            while (true)
            {
                Actor actor = _actorsPool.GetActorFromPool(_spawnActorsType[_currentActorIndex]);

                float randomPoint = _randomizer.GetRandomRangeFloat(0.0f, _spawnAdgeLength);

                actor.ThisTransform.position = _spawnTransform.position + _spawnTransform.right * randomPoint;
                actor.ThisTransform.SetParent(_spawnTransform);

                actor.Movement.SetDestinationTarget(_targetTransform);

                _currentActorIndex++;

                if (_currentActorIndex >= _spawnActorsType.Length)
                    _currentActorIndex = 0;

                yield return new WaitForSeconds(_interval + _randomizer.GetRandomRangeFloat(0.0f, _offsetInterval));
            }
        }

    }
}