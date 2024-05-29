using UnityEngine;
using UnityEngine.Networking;
using Fusion;

public class BoatFloater : NetworkBehaviour {
    
    [SerializeField] Rigidbody rb;
    [SerializeField] int floatersAmount = 1;
    [SerializeField] float depthBeforeSubmerged = 1f;
    [SerializeField] float displacementAmount = 3f;
    [SerializeField] float waterDrag = 1f;
    [SerializeField] float waterAngularDrag = 0.5f;

    public override void FixedUpdateNetwork()
    {
        rb.AddForceAtPosition(Physics.gravity / floatersAmount, transform.position, ForceMode.Acceleration);
        float waveHeight = LowPolyWater.LowPolyWater.Instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}