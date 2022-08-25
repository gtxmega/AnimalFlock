using Events;
using Events.Entity;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Components
{
    public class GroundChecker : MonoBehaviour, IEntityListener
    {
        public bool IsGrounded => _isGrounded;

        [SerializeField] private Transform _thisTransform;
        [SerializeField] private LayerMask _groundLayer;

        private const float RAY_DISTANCE = 15.0f;
        private RaycastHit hit;

        private bool _isGrounded;

        private EntityEvents _entityEvents;
        private IGameEventsListener _gameEventsListener;

        private bool _checkerEnable;


        [Inject]
        private void Construct(IGameEventsListener gameEventsListener)
        {
            _gameEventsListener = gameEventsListener;

            _gameEventsListener.LevelLose += OnLevelLose;
        }

        public void InitializeListener(EntityEvents entityEvents)
        {
            _entityEvents = entityEvents;
        }

        private void OnLevelLose()
        {
            _checkerEnable = false;
        }

        public void StartChecker()
        {
            _checkerEnable = true; 
        }

        private void Update()
        {
            if(_checkerEnable)
            {
                Ray ray = new Ray(_thisTransform.position, _thisTransform.up * -1.0f);

                if (Physics.Raycast(ray, out hit, RAY_DISTANCE, _groundLayer))
                {
                    _isGrounded = true;
                }
                else
                {
                    _isGrounded = false;
                    _entityEvents.OnLeftGround();
                    _checkerEnable = false;
                }
            }
        }


        private void OnDestroy()
        {
            if (_gameEventsListener != null)
            {
                _gameEventsListener.LevelLose -= OnLevelLose;
            }
        }
    }
}