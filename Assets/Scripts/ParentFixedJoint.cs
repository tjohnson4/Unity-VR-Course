using UnityEngine;
using System.Collections;
using System;

public class ParentFixedJoint : MonoBehaviour {

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    FixedJoint fixedJoint;

    public Transform sphere;
    public Rigidbody rigidbodyAttachPoint;

	// Use this for initialization
	void Awake () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        
        // Reset object position on PressUp of the TouchPad
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("You pressed up on the TouchPad");

            sphere.transform.position = new Vector3(0, 0, 0);
            sphere.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
	}

    void OnTriggerStay(Collider col) {
        Debug.Log("You have collided with '"+col.name+"' and activated OnTriggerStay");
    
        if (fixedJoint == nulli && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            fixedJoint = col.gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = rigidbodyAttachPoint;           
        } else if (fixedJoint != null && device.GetTouchUp(SteamVR_Controller.ButtonMast.Trigger)) {
            GameObject go = fixedJoint.gameObject;
            Rigidbody rigidbody = go.GetComponent<Rigidbody>();

            Object.destroy(fixedJoint);
            fixedJoint = null;

            tossObject(rigidbody);
        }
    }

    /**
    * Applies controller movement 'velocity' to bound object on TouchUp. This 
    * is how you throw an object.
    */
    private void tossObject(Rigidbody rigidBody) {
        Transform origin = trackedObject.origin ? trackedObject.origin : trackedObject.transform.parent;
        
        if (origin != null) {
            rigidBody.velocity = origin.TransformVector(device.velocity);
            rigidBody.angularVelocity = origin.TransformVector(device.angularVelocity);
        } else {
            rigidBody.velocity = device.velocity;
            rigidBody.angularVelocity = device.angularVelocity;
        }
    }
}
