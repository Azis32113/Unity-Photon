using UnityEngine;

public static class Utils {
    public static Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-20, 20), 3, Random.Range(-20, 20));
    } 
}