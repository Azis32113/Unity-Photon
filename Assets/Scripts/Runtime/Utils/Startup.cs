using UnityEngine;
public class Startup 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InstantiatePrefabs()
    {
        Debug.Log("-- Instantiating Objects --");

        GameObject[] prefabsToInstantiate = Resources.LoadAll<GameObject>("InstantiateOnLoad/");

        foreach (var prefab in prefabsToInstantiate)
        {
            Debug.Log($"Creating {prefab.name}");

            GameObject.Instantiate(prefab);
        }
        Debug.Log("-- Instantiating Objects Done --");
    }
}