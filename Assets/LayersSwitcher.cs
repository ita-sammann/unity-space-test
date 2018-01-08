using System.Collections.Generic;
using UnityEngine;

public class LayersSwitcher : MonoBehaviour {

    public Transform farLayerContainer;

    public float threshold = 100000.0f;
    protected float sqrThreshold;
    protected float sqrReverseThreshold;

    public float layerScale = 0.0001f;

    public int mainLayerID = 0;
    public int farLayerID = 8;

    public GameObject farCamera;
    protected int farCameraInstanceID;

    private Dictionary<int, Vector3> objScales = new Dictionary<int, Vector3>();

    protected void SetLayerRecursively(GameObject obj, int newLayer) {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private void Start() {
        sqrThreshold = threshold * threshold;
        sqrReverseThreshold = (threshold * layerScale) * (threshold * layerScale);
        farCameraInstanceID = farCamera.GetInstanceID();
    }

    private void LateUpdate() {
        // Moving distant objects to farLayer
        foreach (Transform child in transform) {
            if (child.position.sqrMagnitude > sqrThreshold) {
                int objID = child.gameObject.GetInstanceID();

                child.parent = farLayerContainer;

                if (objScales.ContainsKey(objID)) {
                    objScales[objID] = child.localScale;
                } else {
                    objScales.Add(objID, child.localScale);
                }
                child.localScale = child.localScale * layerScale;

                child.position = child.position * layerScale;

                SetLayerRecursively(child.gameObject, farLayerID);
            }
        }

        // Moving close objects to mainLayer
        foreach (Transform child in farLayerContainer) {
            int objID = child.gameObject.GetInstanceID();
            if (objID != farCameraInstanceID && child.position.sqrMagnitude < sqrReverseThreshold) {
                child.parent = transform;

                if (objScales.ContainsKey(objID)) {
                    child.localScale = objScales[objID];
                } else {
                    child.localScale = child.localScale / layerScale;
                    Debug.LogErrorFormat("Original scale of object %d was not found while moving to main layer", objID);
                }
                child.position = child.position / layerScale;
                SetLayerRecursively(child.gameObject, mainLayerID);
            }
        }

    }
}
