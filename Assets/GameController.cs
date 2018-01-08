using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform mainLayerContainer;
    public GameObject bigCube;

	// Use this for initialization
	void Start () {
        int num = 0;
        for (int i = 10000; i < 100000000; i += 20000) {
            Instantiate(bigCube, new Vector3(0, 0, i), Random.rotation, mainLayerContainer);
            num++;
        }
        Debug.Log("total objects: " + num);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
