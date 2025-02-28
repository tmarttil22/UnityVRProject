using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public Hand hand;

    [SerializeField] private InputActionProperty selectAction;
    [SerializeField] private InputActionProperty activateAction;

    void Start()
    {
        gameObject.SetActive(true);
        selectAction.action.Enable();
        activateAction.action.Enable();
    }

    void Update()
    {
        if (hand == null)
        {
            Debug.LogError("Hand is null!");
            return;
        }

        if (selectAction.action == null)
        {
            Debug.LogError("Select action is null!");
            return;
        }

        if (activateAction.action == null)
        {
            Debug.LogError("Activate action is null!");
            return;
        }

        float gripValue = selectAction.action.ReadValue<float>();
        float triggerValue = activateAction.action.ReadValue<float>();

        hand.SetGrip(gripValue);
        hand.SetTrigger(triggerValue);
    }
}