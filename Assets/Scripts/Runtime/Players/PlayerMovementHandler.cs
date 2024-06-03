
using Fusion;
using UnityEngine;

public class PlayerMovementHandler : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] private int rotateSpeed = 2;
    [SerializeField] private int maxSpeed = 15;
    [SerializeField] private float angle;
    
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        // get input
        // getting the input from the user, putting it in a 'input' var
        // also, if this is not the current player (its a remote playe object) - leaving this script
        if (!GetInput<NetInput>(out var input)) return;

        float movement;
        if (input.Vertical < 0) movement = input.Vertical * 0.2f;
        else movement = input.Vertical;

        rb.velocity = Vector3.Lerp(rb.velocity, maxSpeed * movement * transform.forward, Time.fixedDeltaTime);

        Vector3 rotationSpeed = new Vector3(0, rotateSpeed * Mathf.Clamp(rb.velocity.magnitude / maxSpeed * input.Horizontal, -1, 1), 0);
        Debug.Log(rb.velocity.magnitude);
        transform.Rotate(rotationSpeed, Space.Self);
    } 
}