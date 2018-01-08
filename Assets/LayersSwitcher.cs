using System.Collections.Generic;
using UnityEngine;

public class LayersSwitcher : MonoBehaviour {

    public Transform farLayerContainer;

    public float threshold = 100000f;
    protected float sqrThreshold;
    protected float sqrReverseThreshold;

    public float layerScale = 0.0001f;

    public int mainLayerID = 0;
    public int farLayerID = 8;

    [Tooltip("Dont't forget to add far camera here.")]
    public List<GameObject> farObjectsToIgnore;
    protected List<int> farObjIDsToIgnore;

    public struct ObjectProps {
        public Vector3 scale;
        public bool physical;
    }

    private Dictionary<int, ObjectProps> objectPropsDict = new Dictionary<int, ObjectProps>();

    protected void SetLayerRecursively(GameObject obj, int newLayer) {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private void Start() {
        sqrThreshold = threshold * threshold;
        sqrReverseThreshold = (threshold * layerScale) * (threshold * layerScale);
        farObjIDsToIgnore = new List<int>();
        foreach (GameObject o in farObjectsToIgnore) {
            farObjIDsToIgnore.Add(o.GetInstanceID());
        }
    }

    private void LateUpdate() {
        // Moving distant objects to farLayer
        foreach (Transform child in transform) {
            if (child.position.sqrMagnitude > sqrThreshold) {
                int objID = child.gameObject.GetInstanceID();
                ObjectProps childProps = new ObjectProps() {
                    scale = child.localScale,
                    physical = false,
                };

                Debug.LogFormat("moving object with scale {0}:{1}:{2} to far layer.", child.localScale.x, child.localScale.y, child.localScale.z);
                // Normal rigidbodies go nuts after rescaling
                Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                if (rb != null) {
                    Debug.Log("Has rigidbody: " + rb);
                    childProps.physical = !rb.isKinematic;
                    rb.Sleep();
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                if (objectPropsDict.ContainsKey(objID)) {
                    objectPropsDict[objID] = childProps;
                } else {
                    objectPropsDict.Add(objID, childProps);
                }

                child.localScale = child.localScale * layerScale;
                child.position = child.position * layerScale;
                child.parent = farLayerContainer;

                SetLayerRecursively(child.gameObject, farLayerID);
            }
        }

        // Moving close objects to mainLayer
        foreach (Transform child in farLayerContainer) {
            int objID = child.gameObject.GetInstanceID();

            if (child.position.sqrMagnitude < sqrReverseThreshold && !farObjIDsToIgnore.Contains(objID)) {
                child.parent = transform;
                child.position = child.position / layerScale;

                if (objectPropsDict.ContainsKey(objID)) {
                    child.localScale = objectPropsDict[objID].scale;

                    // Make rigidbody physical again if applicable
                    Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                    if (rb != null) {
                        rb.isKinematic = !objectPropsDict[objID].physical;
                        rb.WakeUp();
                    }
                } else {
                    child.localScale = child.localScale / layerScale;
                    Debug.LogErrorFormat("Original scale of object {0} was not found while moving to main layer", objID);
                }

                SetLayerRecursively(child.gameObject, mainLayerID);
            }
        }

    }
}
