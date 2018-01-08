using UnityEngine;

public class CameraSync : MonoBehaviour {

    public Transform mainCamera;
    public float layerScale = 0.0001f;

    private void LateUpdate() {
        transform.rotation = mainCamera.rotation;
        transform.position = mainCamera.position * layerScale;
    }

}
