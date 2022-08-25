using System;

using UnityEngine;

namespace Events.Entity
{
    public class EntityEvents : MonoBehaviour
    {
        public event Action Spawned;
        public event Action Respawn;

        public event Action LeftGround;

        public event Action OverlapActor;

        public event Action Death;


        public void InitializeEntityListener()
        {
            foreach (var entityListener in gameObject.GetComponentsInChildren<IEntityListener>())
            {
                entityListener.InitializeListener(this);
            }
        }

        public void OnSpawned()
        {
            Spawned?.Invoke();
        }

        public void OnDeath()
        {
            Death?.Invoke();
        }

        public void OnRespawn()
        {
            Respawn?.Invoke();
        }

        public void OnLeftGround()
        {
            LeftGround?.Invoke();
        }

        public void OnOverlapActor()
        {
            OverlapActor?.Invoke();
        }
    }
}