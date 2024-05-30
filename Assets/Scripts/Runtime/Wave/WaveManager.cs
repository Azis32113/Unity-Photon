using Fusion;
using UnityEngine;

public class WaveManager : NetworkBehaviour 
{
    public static WaveManager Instance;

    [SerializeField] float amplitude = 0;
    [SerializeField] float waveSpeed = 0.5f;
    [SerializeField] float waveLength = 0.75f;
    
    private float offset;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public override void Render()
    {
        offset += Time.deltaTime * waveSpeed;
    }

    public float GetWaveHeight(float xPos)
    {
        return amplitude * Mathf.Sin((xPos / waveLength) + offset);
    }
}