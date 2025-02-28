using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    
    /* 
        ATTENTION: For-loops start at index 1 because index 0 includes the *parent* transform
        ("GetComponentsInChildren<>()" but it still gives the parent one??)
    */

    public GameObject targets;
    public GameObject protectiveWall;
    public InputActionReference action;
    private int axeCounter = 0;
    private int shotCounter = 0;
    private int gameCounter = 0;

    private bool gameEndFlag;
    private bool positionsSaved = false;

    // Stopwatch / timescore variables
    bool stopwatchActive = false;
    float currentTime;
    public Text currentTimeText;
    int score;
    public Text scoreText;
    //11 flags as a cheap fix (and maybe a little buggy if some axe manages to take 2 rotations but didnt see it happen, im tired and its past DL) to account for what i think is the parent object of the targets hogging one somehow
    private bool[] flagArray = new bool[11];

    Transform[] axeTransforms;
    Vector3[] axeOriginalLocations = new Vector3[10];
    Quaternion[] axeOriginalRotations = new Quaternion[10];

    // Very boring very long setup code for game
    void Start()
    {
        gameEndFlag = true;

        axeTransforms = targets.GetComponentsInChildren<Transform>();

        // Initialize arrays based on the number of axes
        axeOriginalLocations = new Vector3[axeTransforms.Length];
        axeOriginalRotations = new Quaternion[axeTransforms.Length];
    }

    public void InitializeGame() {

            Debug.Log("STARTGAME DOES GET LAUNCHED");

            // Set all flags to false, in case a game was already played
            for (int i = 0; i < flagArray.Length; i++) {
                flagArray[i] = false;
            }

            gameEndFlag = false;
            axeCounter = 0;
            shotCounter = 0;
            currentTime = 0;
            StartStopwatch();

            // If no game has started, axes are guaranteed to be in correct positions and rotations (Player can't access them or knock them over with cannon since they start kinematic)
            if (!positionsSaved) {
                for (int i = 1; i < axeTransforms.Length; i++) {
                    axeTransforms[i].GetLocalPositionAndRotation(out axeOriginalLocations[i], out axeOriginalRotations[i]);
                }
                positionsSaved = true;
            }

            // If a game has been started previously, use the confirmed correct positions and rotations to reset the axes
            for (int i = 1; i < axeTransforms.Length; i++) {
                axeTransforms[i].SetPositionAndRotation(axeOriginalLocations[i], axeOriginalRotations[i]);

                //Stop movement in case axes are still moving when player replays
                axeTransforms[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero; 
                axeTransforms[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

            // Remove protective wall to allow the player to start shooting
            protectiveWall.SetActive(false);

            //Game initialized, increase game count
            gameCounter++;
    }

    void Update()
    {
        if (action.action.triggered) {
            shotCounter++;
        }
        if (stopwatchActive) {
            currentTime += Time.deltaTime;
        }

        Transform[] axeTransforms = targets.GetComponentsInChildren<Transform>();

        // Check each axe if they have fallen more than 45 degrees, and if they have, remove its flag from the ones standing up and add to axeCounter
        for (int i = 1; i < axeTransforms.Length; i++) {
            if (!flagArray[i]) {
                float xRot = axeTransforms[i].rotation.eulerAngles.x;
                float zRot = axeTransforms[i].rotation.eulerAngles.z;

                //Normalize the angles
                if (xRot > 180) xRot -= 360;
                if (zRot > 180) zRot -= 360;

                if (Mathf.Abs(xRot) >= 45 || Mathf.Abs(zRot) >= 45) {
                    flagArray[i] = true;
                    axeCounter++;
                }
            } 
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");

        CheckGameValidity();
        Debug.Log("Amount of axes felled " + axeCounter);
        if (gameEndFlag) {
            score = Mathf.RoundToInt(currentTime * shotCounter / 10);
            scoreText.text = "Your score: " + score.ToString();
        }
    }



    public void StartStopwatch() {
        stopwatchActive = true;
    }

    public void StopStopwatch() {
        stopwatchActive = false;
    }

    private void CheckGameValidity() {
        if(axeCounter >= 10 && !gameEndFlag) {
            StopStopwatch();
            gameEndFlag = true;
        }
    }
}
