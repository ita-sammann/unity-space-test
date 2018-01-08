using UnityEngine;

//[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour {

    [System.Serializable]
    public struct Layer {
        public Transform container;
        public float scale;
    }

    public float threshold = float.MaxValue;
    public Layer[] layers;

    private float sqrThreshold;

    protected void MoveObjects(Layer layer, Vector3 cameraPosition) {
        foreach (Transform t in layer.container) {
            t.position -= cameraPosition * layer.scale;
        }
    }

    private void Start() {
        // To avoid usage of expensive sqrt operation
        sqrThreshold = threshold * threshold;
    }

    private void LateUpdate() {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;

        if (cameraPosition.sqrMagnitude > sqrThreshold) {
            foreach (Layer l in layers) {
                MoveObjects(l, cameraPosition);
            }
        }
    }
}
