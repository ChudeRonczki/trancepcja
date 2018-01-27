using System.Collections;
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
    public float TurnForce = 0.1f;
    [SerializeField]
    public float MoveForceSingleWing = 1.0f;
    [SerializeField]
    public float PushForceSingleWing = 1.0f;
    [SerializeField]
    public float MoveForceBothWings = 5.0f;

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

        if (Input.GetButtonDown("Jump"))
        {
            Rigidbody.AddTorque(transform.forward * 10f, ForceMode.Impulse);
            Rigidbody.AddForce(Vector3.up * 100f, ForceMode.Impulse);
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
            Rigidbody.AddTorque(transform.forward * TurnForce, ForceMode.Impulse);
            var moveVector = Vector3.up * PushForceSingleWing + transform.forward * MoveForceSingleWing;
            Rigidbody.AddForce(moveVector, ForceMode.Impulse);
            Debug.Log("Left");
        }
        else if (action == FlapAction.FlapRight)
        {
            Rigidbody.AddTorque(-transform.forward * TurnForce, ForceMode.Impulse);
            var moveVector = Vector3.up * PushForceSingleWing + transform.forward * MoveForceSingleWing;
            Rigidbody.AddForce(moveVector, ForceMode.Impulse);
            Debug.Log("Right");
        }
        else if (action == FlapAction.FlapBoth)
        {
            Debug.Log("Both");
            var moveVector = Vector3.up * PushForceSingleWing + transform.up * MoveForceBothWings;
            Rigidbody.AddForce(moveVector, ForceMode.Impulse);
        }

        if (action != FlapAction.None)
        {
            LastFlapTime = Time.time;
        }
    }
}