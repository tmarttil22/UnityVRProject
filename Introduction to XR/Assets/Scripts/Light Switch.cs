using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light light;
    public InputActionReference action;
    void Start()
    {
        light = GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) => 
        {
            light.color = new Color(1, 0.92f, 0.016f, 1);
            light.intensity = 50;
        };
    }
}
