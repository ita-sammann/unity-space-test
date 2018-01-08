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
        float vertical = 0f;
        if (Input.GetKey(KeyCode.Space)) {
            vertical = 1f;
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            vertical = -1f;
        }
        rb.AddRelativeForce(sideways * thrust, vertical * thrust, forward * thrust);

        if (rb.velocity.sqrMagnitude > sqrMaxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
