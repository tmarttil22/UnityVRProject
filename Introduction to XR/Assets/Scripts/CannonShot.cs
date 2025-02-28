using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonShot : MonoBehaviour
{

    public GameObject cannonball;
    public Transform spawnLocation;
    public InputActionReference action;
    public float yVelocity = 10f;
    private GameObject newCannonball;
    private Rigidbody body;
    public Hand hand;
    private Rigidbody handBody;

    void Start()
    {
        action.action.Enable();
        
    }

    void Update()
    {
        CannonShoot();
    }

    void CannonShoot() {
        if(action.action.triggered) {
            newCannonball = Instantiate(cannonball, spawnLocation.position, spawnLocation.rotation);
            body = newCannonball.GetComponent<Rigidbody>();
            handBody = hand.GetComponent<Rigidbody>();
            
            newCannonball.transform.rotation = spawnLocation.rotation;

            if (body != null) {
                body.freezeRotation = true;
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;

                // Up is the direction of the cannon barrel
                body.linearVelocity = (spawnLocation.up * yVelocity) + handBody.linearVelocity;
            }
        }
    }
}
