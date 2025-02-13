using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Start()
    {
        action.action.Enable();

        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        grabbing = action.action.IsPressed();

        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
            {
                if (nearObjects.Count > 0)
                {
                    grabbedObject = nearObjects[0];

                    Rigidbody rb0 = grabbedObject.GetComponent<Rigidbody>();
                    if (rb0) {
                        rb0.useGravity = false;
                        rb0.isKinematic = true;
                    }
                }
                else
                {
                    grabbedObject = otherHand.grabbedObject;
                }
            }
            if (grabbedObject)
            {
                if (otherHand.grabbedObject == grabbedObject)
                {
                    grabbedObject.position = (transform.position + otherHand.transform.position) / 2;
                    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Quaternion.Slerp.html
                    grabbedObject.rotation = Quaternion.Slerp(transform.rotation, otherHand.transform.rotation, 0.5f);
                }

                else {
                    // Change these to add the delta position and rotation instead
                    // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                    Vector3 deltaPos = transform.position - lastPosition;
                    Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);
                    Vector3 linearVector = grabbedObject.position - transform.position;
                    Vector3 angularVector = deltaRot * linearVector - linearVector;

                    grabbedObject.position = transform.position + deltaPos + angularVector;
                    grabbedObject.rotation = deltaRot * grabbedObject.rotation;
                }
            }
        }
        // If let go of button, release object
        else if (grabbedObject) {
            Rigidbody rb2 = grabbedObject.GetComponent<Rigidbody>();
            if (rb2)
            {
                // Reactivate gravity and disable kinematic when user no longer holds the object
                rb2.useGravity = true;
                rb2.isKinematic = false;
            }
            grabbedObject = null;                
            otherHand.grabbedObject = null;
        }

        // Should save the current position and rotation here
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}