using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;

    private byte[] connectionToken;

    private void Awake() 
    {
        if (Instance == null) Instance = this;
        else 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        if (connectionToken == null) 
        {
            connectionToken = ConnectionTokenUtils.NewToken();
            Debug.Log($"Player connection token {ConnectionTokenUtils.HashToken(connectionToken)}");
        }
    }

    public void SetConnectionToken(byte[] connectionToken)
    {
        this.connectionToken = connectionToken;
    }

    public byte[] GetConnectionToken()
    {
        return connectionToken;
    }


}