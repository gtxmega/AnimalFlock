using Components;
using Events;
using Events.Entity;
using UnityEngine;
using Zenject;
using DG.Tweening;
using System.Collections;
using Services.Confetti;

namespace Data.Types
{
    public class Actor : MonoBehaviour, IEntityListener
    {
        public EntityEvents EntityEvents => _entityEvents;
        public Movement Movement => _movement;
        public Transform ThisTransform => _thisTransform;
        public EActorType ActorType => _actorType;
        public bool IsDead => _isDead;

        public int NodeIndex => _nodeIndex;

        [SerializeField] private Transform _thisTransform;
        [SerializeField] private EActorType _actorType;

        [Header("Death parameters")]
        [SerializeField] private float _deadDelay;
        [SerializeField] private float _bounceSize;

        [Header("Death VFX")]
        [SerializeField] private EConfettiColor _deadConfettiColor;
        [SerializeField] private ParticleSystem _vfxDead;

        private int _nodeIndex;
        private bool _isDead;

        //Components
        private Movement _movement;
        private GroundChecker _groundChecker;

        //Services
        private EntityEvents _entityEvents;
        private IGameEventsExecuter _gameEventsExecuter;
        private IGameEventsListener _gameEventsListener;
        private IConfettiService _confettiService;

        [Inject]
        private void Construct(Movement movement, GroundChecker groundChecker,
            IGameEventsExecuter gameEventsExecuter, IGameEventsListener gameEventsListener, 
            IConfettiService confettiService)
        {
            _movement = movement;
            _groundChecker = groundChecker;

            _gameEventsExecuter = gameEventsExecuter;
            _gameEventsListener = gameEventsListener;
            _confettiService = confettiService;

            _gameEventsListener.LevelLose += OnLevelLose;
        }

        private void OnLevelLose()
        {
            _movement.ForceStop();
        }

        public void InitializeListener(EntityEvents entityEvents)
        {
            _entityEvents = entityEvents;

            _entityEvents.Respawn += OnRespawn;
            _entityEvents.LeftGround += OnLeftGround;
        }


        private void OnRespawn()
        {
            _isDead = false;
            _thisTransform.localScale = Vector3.one;

            gameObject.SetActive(true);

            _entityEvents.LeftGround += OnLeftGround;
        }

        public void SetNodeIndex(int index) => _nodeIndex = index;

        private void OnLeftGround()
        {
            _isDead = true;

            _thisTransform.SetParent(null);
            _movement.ForceStop();
            StartCoroutine(DeadProcess());

            _gameEventsExecuter.OnActorLeftGround(this);
        }

        public void OnDead()
        {
            if(_vfxDead != null)
                _vfxDead.Play();

            _isDead = true;

            Vector3 scale = _thisTransform.localScale;

            scale.x += _bounceSize;
            scale.y += _bounceSize;
            scale.z += _bounceSize;

            _thisTransform
                .DOScale(scale, _deadDelay)
                .SetEase(Ease.InOutBounce)
                .OnComplete( () => 
                { 
                    _confettiService.PlayConfettiAt(_thisTransform.position, _deadConfettiColor);
                });

            StartCoroutine(DeadProcess());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isDead) return;

            _groundChecker.StartChecker();

            if (other.TryGetComponent<Actor>(out Actor actor))
            {
                if (actor.IsDead == false)
                {
                    _entityEvents.OnOverlapActor();

                    _gameEventsExecuter.OnActorCollisionActor(this, actor);
                    _movement.Stop();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isDead) return;

            if (other.TryGetComponent<Actor>(out Actor actor))
            {
                if(actor.IsDead == false)
                    _gameEventsExecuter.OnActorCollisionExitActor(this, actor);
            }
        }

        private IEnumerator DeadProcess()
        {
            yield return new WaitForSeconds(_deadDelay);
            
            gameObject.SetActive(false);

            _entityEvents.LeftGround -= OnLeftGround;

            _entityEvents.OnDeath();
            _gameEventsExecuter.OnActorDied(this);
        }

        private void OnDestroy()
        {
            if (_entityEvents != null)
            {
                _entityEvents.Respawn -= OnRespawn;
                _entityEvents.LeftGround -= OnLeftGround;
            }
        }

    }
}