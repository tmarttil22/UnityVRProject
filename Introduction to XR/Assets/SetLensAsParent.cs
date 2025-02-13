using UnityEngine;

public class SetLensAsParent : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform lens;

    public Transform camera;
    void Start()
    {
        camera.SetParent(lens);
    }

    // Update is called once per frame
    void Update()
    {
        camera.SetParent(lens);
        camera.position = lens.position;
    }
}
