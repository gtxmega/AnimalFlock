using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Services.Confetti
{
    public class ConfettiService : MonoBehaviour, IConfettiService
    {
        [SerializeField] private ParticleSystem _confettiBlue;
        [SerializeField] private ParticleSystem _confettiGreenYellow;
        [SerializeField] private ParticleSystem _confettiRainbow;
        [SerializeField] private ParticleSystem _confettiRed;

        [Space]
        [SerializeField] private Transform _thisTransform;

        private const int CONFETTI_POOL_SIZE = 20;

        private Dictionary<EConfettiColor, Queue<ParticleSystem>> _confettiPools;
        private Dictionary<EConfettiColor, ParticleSystem> _confettiPrefabStorage;

        private IGameEventsListener _gameEventsListener;

        private Dictionary<EConfettiColor, List<ParticleSystem>> _activeParticals;
        private Coroutine _particalReturnCoroutine;
        private int _currentActiveParticals;

        [Inject]
        private void Construct(IGameEventsListener gameEventsListener)
        {
            _gameEventsListener = gameEventsListener;

            _gameEventsListener.LevelBootstrap += OnLevelBootstrap;
        }

        private void OnLevelBootstrap()
        {
            FillConfettiStorage();

            _activeParticals = new Dictionary<EConfettiColor, List<ParticleSystem>>()
            {
                [EConfettiColor.Blue] = new List<ParticleSystem>(),
                [EConfettiColor.GreenYellow] = new List<ParticleSystem>(),
                [EConfettiColor.Red] = new List<ParticleSystem>(),
                [EConfettiColor.Rainbow] = new List<ParticleSystem>(),
            };

            _confettiPools = new Dictionary<EConfettiColor, Queue<ParticleSystem>>();

            CreatePool(EConfettiColor.Blue);
            CreatePool(EConfettiColor.GreenYellow);
            CreatePool(EConfettiColor.Red);
            CreatePool(EConfettiColor.Rainbow);

            _confettiPrefabStorage.Clear();

        }

        public void PlayConfettiAt(Vector3 position, EConfettiColor confettiColor)
        {
            ParticleSystem confetti = GetObjectFromPool(confettiColor);

            confetti.transform.position = position;
            confetti.gameObject.SetActive(true);
            confetti.Play();

            _currentActiveParticals++;

            _activeParticals[confettiColor].Add(confetti);

            if (_particalReturnCoroutine == null)
                _particalReturnCoroutine = StartCoroutine(ParticalDisable());
        }

        private IEnumerator ParticalDisable()
        {
            while (true)
            {
                foreach (KeyValuePair<EConfettiColor, List<ParticleSystem>> confetti in _activeParticals)
                {
                    for (int i = 0; i < confetti.Value.Count; i++)
                    {
                        if (confetti.Value[i].isPlaying == false)
                        {
                            confetti.Value[i].gameObject.SetActive(false);

                            _confettiPools[confetti.Key].Enqueue(confetti.Value[i]);

                            confetti.Value.RemoveAt(i);

                            _currentActiveParticals--;
                        }
                    }
                }

                if (_currentActiveParticals == 0)
                {
                    _particalReturnCoroutine = null;
                    yield break;
                }

                yield return null;
            }
        }

        private ParticleSystem GetObjectFromPool(EConfettiColor confettiColor)
        {
            if (_confettiPools[confettiColor].Count == 0)
            {
                AddNewObjectToPool(confettiColor);

                return GetObjectFromPool(confettiColor);
            }

            return _confettiPools[confettiColor].Dequeue();
        }

        private void CreatePool(EConfettiColor confettiColor)
        {
            _confettiPools.Add(confettiColor, new Queue<ParticleSystem>());

            for (int i = 0; i < CONFETTI_POOL_SIZE; ++i)
            {
                AddNewObjectToPool(confettiColor);
            }
        }

        private void AddNewObjectToPool(EConfettiColor confettiColor)
        {
            ParticleSystem confetti = Instantiate(_confettiPrefabStorage[confettiColor], _thisTransform);

            confetti.gameObject.SetActive(false);

            _confettiPools[confettiColor].Enqueue(confetti);
        }

        private void FillConfettiStorage()
        {
            _confettiPrefabStorage = new Dictionary<EConfettiColor, ParticleSystem>();

            _confettiPrefabStorage.Add(EConfettiColor.Blue, _confettiBlue);
            _confettiPrefabStorage.Add(EConfettiColor.Red, _confettiRed);
            _confettiPrefabStorage.Add(EConfettiColor.Rainbow, _confettiRainbow);
            _confettiPrefabStorage.Add(EConfettiColor.GreenYellow, _confettiGreenYellow);
        }
    }
}