using System.Xml;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    private static CameraFollow _instance;
    public static CameraFollow Instance {
        get { return _instance ;}
        set 
        { 
            if (value == null) _instance = null;
            else if (_instance == null) _instance = value;
            else if (_instance != value) 
            {
                Destroy(value);
            }
        }
    }

    private Transform target;

    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() 
    {
        if (Instance == this) Instance = null;
    }

    private void LateUpdate() 
    {
        if (target != null)
        {
            transform.SetPositionAndRotation(target.position, target.rotation);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}