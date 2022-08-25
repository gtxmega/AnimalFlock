
using UnityEngine;

namespace Services.Factory
{
    public interface IGameFactory
    {
        GameObject CreateGameObject(string path);
        GameObject CreateGameObject(string path, Transform parent);
        GameObject CreatePrefab(GameObject prefab);
        GameObject CreatePrefab(GameObject prefab, Vector3 spawnPosition);
        GameObject CreateGameObject(string path, Vector3 at);
        T CreateOf<T>();
        GameObject CreateGameObject(GameObject prefabObject, Transform parent);
    }
}