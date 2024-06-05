using Fusion;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(NetworkObject))]
public class WaterManager : NetworkBehaviour 
{
    private MeshFilter meshFilter;

    public override void Spawned()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public override void Render()
    {
        if (WaveManager.Instance == null) return;
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.Instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();   
    }
}