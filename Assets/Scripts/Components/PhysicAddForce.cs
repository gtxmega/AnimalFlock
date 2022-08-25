using Events.Entity;
using Services.Assets;
using System.Collections;
using UnityEngine;

namespace Scripts.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicAddForce : MonoBehaviour, IEntityListener
    {
        [SerializeField] private Transform _ownerTransform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _forcePower;

        private Transform _centerAreaTransform;

        private EntityEvents _entityEvents;

        public void InitializeListener(EntityEvents entityEvents)
        {
            _entityEvents = entityEvents;

            _entityEvents.Spawned += OnEntitySpawned;
            _entityEvents.LeftGround += EntityOnLeftGround;
        }

        private void OnEntitySpawned()
        {
            GameObject centerAreaObject = GameObject.FindGameObjectWithTag(AssetsPath.TARGET_MOVE_TAG);

            _centerAreaTransform = centerAreaObject.GetComponent<Transform>();
        }

        private void EntityOnLeftGround()
        {
            _ownerTransform.SetParent(null);

            Vector3 direction = _ownerTransform.position - _centerAreaTransform.position;

            direction.Normalize();

            _rigidbody.AddForce(direction * _forcePower, ForceMode.Impulse);
        }

        private void OnDestroy()
        {
            if (_entityEvents != null)
            {
                _entityEvents.Spawned -= OnEntitySpawned;
                _entityEvents.LeftGround -= EntityOnLeftGround;
            }
        }
    }
}