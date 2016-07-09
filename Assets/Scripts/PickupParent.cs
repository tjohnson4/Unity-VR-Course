using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    public Transform sphere;

    // Unity Lifecycle Event 
    void Awake () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    // This method is expected to be called 90 frames per second on a VR capable machine.
    void FixedUpdate () {
    	// Controller device
        device = SteamVR_Controller.Input((int)trackedObject.index);
        
        // Reset object position on PressUp of the TouchPad
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("You pressed up on the TouchPad");

            sphere.transform.position = new Vector3(0, 0, 0);
            sphere.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        
        // Stubs for trigger events
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Holding down 'Touch' on the trigger");
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Holding down 'TouchDown' on the trigger");
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Holding down 'TouchUp' on the trigger");
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Holding down 'Press' on the trigger");
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Holding down 'PressDown' on the trigger");
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Holding down 'PressUp' on the trigger");
        }
    }

    // Enables attaching the rigid body object (Picking up the sphere)
    void OnTriggerStay(Collider col) {
        Debug.Log("You have collided with '"+col.name+"' and activated OnTriggerStay");

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("You have collided with '" + col.name + "' while holding down Touch");

            col.attachedRigidbody.isKinematic = true;
            col.gameObject.transform.SetParent(gameObject.transform);
        } 

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("You have released Touch while colliding with '" + col.name);

            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;

            tossObject(col.attachedRigidbody);
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
