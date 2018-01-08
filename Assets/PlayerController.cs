using UnityEngine;

public class PlayerController : MonoBehaviour {

    //private Rigidbody rb;

    public float speed;
    public float rotationSpeed;

    // Use this for initialization
    private void Start () {
        //rb = GetComponent<Rigidbody>();
	}
	
	private void Update () {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }

    private void FixedUpdate() {
        
    }
}
