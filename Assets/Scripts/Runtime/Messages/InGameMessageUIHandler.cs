using UnityEngine;
using TMPro;
using System.Collections;

public class InGameMessageUIHandler : MonoBehaviour 
{    
    [SerializeField] private TextMeshProUGUI[] textFields;

    private Queue messageQueue = new Queue();

    private void Start() {
        ClearMessage();
    }

    public void OnGameMessageReceived(string message)
    {
        Debug.Log($"InGameMessageUIHandler: {message}");

        messageQueue.Enqueue(message);
        
        if (messageQueue.Count > textFields.Length) messageQueue.Dequeue();

        int i = messageQueue.Count - 1;
        foreach (var messageInQueue in messageQueue)
        {
            textFields[i].text = messageInQueue.ToString();
            i--;
        }
    }

    private void ClearMessage()
    {
        int i = 0;
        foreach (var messageInQueue in messageQueue)
        {
            textFields[i].text = "";
            i++;
        }
    }

}