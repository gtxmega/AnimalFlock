using UnityEngine;

using Zenject;

namespace Services.Assets
{
    public class AssetProvider : IAssets
    {
        [Inject] private DiContainer _diContainer;

        private Transform _playerSpawnTransform;


        public GameObject Instantiate(string filePath)
        {
            if (_playerSpawnTransform == null)
            {
                GameObject spawnPoint = GameObject.FindGameObjectWithTag(AssetsPath.PLAYER_SPAWN_OBJECT_TAG);

                _playerSpawnTransform = spawnPoint.transform;
            }

#if UNITY_EDITOR
            if (_playerSpawnTransform == null)
            {
                Debug.Log(AssetsPath.PLAYER_SPAWN_OBJECT_TAG + ": Not founded!");
            }
#endif

            return SpawnFromDI(filePath, _playerSpawnTransform.position);
        }

        public GameObject Instantiate(string filePath, Vector3 at)
        {
            return SpawnFromDI(filePath, at);
        }

        public GameObject Instantiate(GameObject origin, Vector3 at)
        {
            return _diContainer.InstantiatePrefab(origin, at, Quaternion.identity, null);
        }

        public GameObject Instantiate(GameObject prefab, Transform parent)
        {
            return _diContainer.InstantiatePrefab(prefab, parent);
        }

        public T Instantiate<T>()
        {
            return _diContainer.Instantiate<T>();
        }

        private GameObject SpawnFromDI(string path, Vector3 position)
        {
            return _diContainer.InstantiatePrefabResource(path, position, Quaternion.identity, null);
        }
    }
}