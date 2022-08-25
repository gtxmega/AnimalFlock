using System.Collections;
using UnityEngine;

namespace Components
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _velocity;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _stoppingDelay;
        [SerializeField] private Transform _transform;

        private Transform _targetTransform;

        private bool _isMovement;

        public void SetDestinationTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;

            _isMovement = true;
        }

        private void Update()
        {
            if (_isMovement)
            {
                Vector3 direction = _targetTransform.position - _transform.position;
                float distanceToTarget = direction.magnitude;

                Vector3 normalizeDirection = direction / distanceToTarget;

                Vector3 moveVector = _transform.position + normalizeDirection * _velocity * Time.deltaTime;

                _transform.position = moveVector;

                if (distanceToTarget < _stoppingDistance)
                {
                    _transform.SetParent(_targetTransform);
                }
                else
                {
                    _transform.LookAt(_targetTransform.position);
                }
            }
        }

        public void ForceStop()
        {
            _isMovement = false;
        }

        public void Stop()
        {
            _transform.SetParent(_targetTransform);
        }

    }
}