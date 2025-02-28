using UnityEngine;

public class OrbitingMoon : MonoBehaviour
{
    public GameObject Planet;
    public float degreesPerSecond = 3.0f;
    void Update()
    {
        transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0);
    }
}
