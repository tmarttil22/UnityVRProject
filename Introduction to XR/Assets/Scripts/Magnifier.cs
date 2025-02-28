using UnityEngine;

public class Magnifier : MonoBehaviour
{
    
    public Transform playerHead;

    public Transform lensCamera;

    public Transform magnifyingGlass;

    public Transform lens;

    void Start()
    {
        lensCamera.SetParent(playerHead.transform);
    }

    // Update the vector between the middlepoint of the 2 rendered screens and the magnifying lens
    void Update()
    {
        // Ignore z-axis rotation, since that makes the lens (and the video that gets rendered on it) turn
        Vector3 lensRotation = new Vector3(magnifyingGlass.eulerAngles.x, magnifyingGlass.eulerAngles.y, 0);
        Quaternion lensRotationQuat = Quaternion.Euler(lensRotation);
        lens.localRotation = Quaternion.Inverse(magnifyingGlass.rotation) * lensRotationQuat;

        // Calculate the vector from the headset to the lens, then point the camera in the direction of that vector
        Vector3 directionToLens = (lens.position - playerHead.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToLens);
        lensCamera.SetPositionAndRotation(lensCamera.position, targetRotation);

        Quaternion inverser = Quaternion.Inverse(magnifyingGlass.localRotation) * magnifyingGlass.localRotation;
        lens.localRotation = inverser * lens.localRotation;
    }
}
