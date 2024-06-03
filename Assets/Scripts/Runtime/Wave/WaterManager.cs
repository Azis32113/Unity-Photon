using Fusion;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterManager : NetworkBehaviour 
{
    private MeshFilter meshFilter;

    private void Awake() 
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