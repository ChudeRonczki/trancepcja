﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlapAction
{
    None = 0,
    FlapLeft,
    FlapRight,
    FlapBoth
}

public class BatController : MonoBehaviour
{
	[SerializeField]
	public float ControllerDeadzone = 0.5f;
	
    [SerializeField]
    public string PlayerNumber = "1";

    [SerializeField]
    public float TurnForce = 0.5f;
    [SerializeField]
    public float MoveForceSingleWing = 10.0f;
    [SerializeField]
    public float PushForceSingleWing = 13.0f;
    [SerializeField]
    public float MoveForceBothWings = 15.0f;

    bool LeftPressed = false;
    bool RightPressed = false;
    bool BothPressed = false;

    bool LeftChanged = false;
    bool RightChanged = false;

    FlapAction LastAction = FlapAction.None;
    float LastFlapTime = 0f;

    Rigidbody Rigidbody;

    int MaxFramesBetweenInputs = 2;
    int FramesBetweenInputs = 0;

    float Timer = 0f;
    public float InputLag = 0.05f;

    [SerializeField]
    LayerMask LayerMask;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CaptureInput();

        if (Timer > 0f)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0f)
            {
                HandleAction(GetAction());
                ResetInput();
            }
        }
        else if (LeftChanged || RightChanged)
        {
            StartTimer();
        }

        if (Input.GetButtonDown("Drop" + PlayerNumber))
        {
            Rigidbody.AddTorque(transform.forward * 50f, ForceMode.Impulse);
        }
    }

    private FlapAction GetAction()
    {
        bool left = LeftChanged && LeftPressed;
        bool right = RightChanged && RightPressed;
        bool both = left && right;

        if (both) return FlapAction.FlapBoth;
        else if (left) return FlapAction.FlapLeft;
        else if (right) return FlapAction.FlapRight;
        else return FlapAction.None;
    }

    private void StartTimer()
    {
        Timer = InputLag;
    }

    private void CaptureInput()
    {
        bool left = Input.GetAxisRaw("FlapL" + PlayerNumber) > ControllerDeadzone;
        bool right = Input.GetAxisRaw("FlapR" + PlayerNumber) > ControllerDeadzone;

        if (left != LeftPressed)
        {
            if (!LeftChanged)
            {
                LeftChanged = true;
                LeftPressed = !LeftPressed;
            }
        }

        if (right != RightPressed)
        {
            if (!RightChanged)
            {
                RightChanged = true;
                RightPressed = !RightPressed;
            }
        }
    }

    private void ResetInput()
    {
        RightChanged = false;
        LeftChanged = false;
    }

    private void HandleAction(FlapAction action)
    {
        if (action == FlapAction.FlapLeft)
        {
            Flip(1f);
        }
        else if (action == FlapAction.FlapRight)
        {
            Flip(-1f);
        }
        else if (action == FlapAction.FlapBoth)
        {
            var moveVector = Vector3.up * PushForceSingleWing * 0.5f + transform.up * MoveForceBothWings;
            Rigidbody.AddForce(moveVector, ForceMode.Impulse);
        }

        if (action != FlapAction.None)
        {
            LastFlapTime = Time.time;
        }
    }

    private void Flip(float direction)
    {
        Rigidbody.AddTorque(direction * transform.forward * TurnForce, ForceMode.Impulse);
        var moveVector = Vector3.up * PushForceSingleWing + transform.forward * PushForceSingleWing;
        Rigidbody.AddForce(moveVector, ForceMode.Impulse);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, direction * transform.right), out hit, 0.4f, LayerMask))
        {
            Rigidbody.AddTorque(direction * transform.forward * TurnForce * 3f, ForceMode.Impulse);
            Rigidbody.AddForce(Vector3.up * PushForceSingleWing, ForceMode.Impulse);
        }
    }
}