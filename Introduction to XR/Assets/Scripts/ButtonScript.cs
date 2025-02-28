using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    // Followed this tutorial: https://www.youtube.com/watch?v=lPPa9y_czlE
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    private GameObject presser;
    private AudioSource sound;
    private bool isPressed = false;
    private StartGame startGameScript;
    private Vector3 originalPosition;

void Start()
    {
        sound = GetComponent<AudioSource>();
        startGameScript = GameObject.FindGameObjectWithTag("TagStartGame").GetComponent<StartGame>();

        // Save the original position of the button
        originalPosition = button.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = originalPosition + new Vector3(0, -0.012f, 0); // Move button down
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;

            // Ensure the button resets after 0.5s in case OnTriggerExit is not called
            Invoke(nameof(ResetButton), 0.25f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == presser)
        {
            ResetButton();
        }
    }

    private void ResetButton()
    {
        button.transform.localPosition = originalPosition; // Reset to original position
        onRelease.Invoke();
        isPressed = false;
    }

    public void CallStartGame()
    {
        if (startGameScript != null)
        {
            startGameScript.InitializeGame();
            Debug.Log("StartGame ran through ButtonScript");
        }
        else
        {
            Debug.LogWarning("StartGame script reference is missing!");
        }
    }
}