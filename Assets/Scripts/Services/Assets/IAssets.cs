using UnityEngine;


namespace Services.Assets
{
    public interface IAssets
    {
        GameObject Instantiate(string filePath);
        GameObject Instantiate(string filePath, Vector3 at);
        GameObject Instantiate(GameObject origin, Vector3 at);
        T Instantiate<T>();
        GameObject Instantiate(GameObject prefab, Transform parent);
    }
}