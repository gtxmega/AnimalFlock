using Data.Types;
using Services.RandomizerService;
using Zenject;

namespace Services.Spawners
{
    public class ActorSpawnerAfterDead : ActorSpawnerBase
    {
        private int _currentActorIndex;
        private Actor _currentActor;

        private IRandomizer _randomizer;

        [Inject]
        private void Construct(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public override void StartSpawn()
        {
            SpawnActor();
        }

        public override void StopSpawn()
        {
            if(_currentActor != null )
            {
                _currentActor.EntityEvents.OverlapActor -= OnOverlapActor;
                _currentActor = null;
            }
        }

        private void SpawnActor()
        {
            _currentActor = _actorsPool.GetActorFromPool(_spawnActorsType[_currentActorIndex]);

            float randomPoint = _randomizer.GetRandomRangeFloat(0.0f, _spawnAdgeLength);

            _currentActor.ThisTransform.position = _spawnTransform.position + _spawnTransform.right * randomPoint;
            _currentActor.ThisTransform.SetParent(_spawnTransform);

            _currentActor.Movement.SetDestinationTarget(_targetTransform);

            _currentActor.EntityEvents.OverlapActor += OnOverlapActor;
            _currentActor.EntityEvents.Death += OnOverlapActor;

            _currentActorIndex++;

            if (_currentActorIndex >= _spawnActorsType.Length)
                _currentActorIndex = 0;
        }

        private void OnOverlapActor()
        {
            _currentActor.EntityEvents.OverlapActor -= OnOverlapActor;
            _currentActor.EntityEvents.Death -= OnOverlapActor;

            SpawnActor();
        }
    }
}