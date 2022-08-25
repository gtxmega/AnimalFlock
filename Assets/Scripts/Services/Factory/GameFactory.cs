using Services.Assets;
using UnityEngine;


namespace Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private IAssets _assets;

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }


        public GameObject CreateGameObject(string path)
        {
            GameObject actor = _assets.Instantiate(path);

            return actor;
        }

        public GameObject CreateGameObject(GameObject prefabObject, Transform parent)
        {
            return _assets.Instantiate(prefabObject, parent);
        }

        public T CreateOf<T>()
        {
            return _assets.Instantiate<T>();
        }

        public GameObject CreatePrefab(GameObject prefab)
        {
            GameObject instance = _assets.Instantiate(prefab, Vector3.zero);

            return instance;
        }

        public GameObject CreatePrefab(GameObject prefab, Vector3 spawnPosition)
        {
            GameObject instance = _assets.Instantiate(prefab, spawnPosition);

            return instance;
        }

        public GameObject CreateGameObject(string path, Vector3 position)
        {
            GameObject actor = _assets.Instantiate(path, position);

            return actor;
        }

        public GameObject CreateGameObject(string path, Transform parent)
        {
            GameObject actor = _assets.Instantiate(path);
            actor.transform.SetParent(parent);
            actor.transform.localPosition = Vector3.zero;

            return actor;
        }
    }
}