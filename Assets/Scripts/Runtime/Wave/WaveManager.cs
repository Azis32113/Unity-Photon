using Fusion;
using UnityEngine;

public class WaveManager : NetworkBehaviour 
{
    public static WaveManager Instance;

    [SerializeField] float amplitude = 0;
    [SerializeField] float waveSpeed = 0.5f;
    [SerializeField] float waveLength = 0.75f;

    [Networked] public float Offset { get => default; set{} }

    public override void Spawned()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public override void Render()
    {
        Offset += Time.deltaTime * waveSpeed;
    }

    public float GetWaveHeight(float xPos)
    {
        return amplitude * Mathf.Sin((xPos / waveLength) + Offset);
    }
}