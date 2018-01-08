using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;

    public float thrust;
    public float maxSpeed;
    private float sqrMaxSpeed;

    private void Start () {
        rb = GetComponent<Rigidbody>();
        sqrMaxSpeed = maxSpeed * maxSpeed;
    }
	
    private void FixedUpdate() {
        float forward = Input.GetAxis("Vertical");
        float sideways = Input.GetAxis("Horizontal");
        rb.AddRelativeForce(sideways * thrust, 0.0f, forward * thrust);

        if (rb.velocity.sqrMagnitude > sqrMaxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
