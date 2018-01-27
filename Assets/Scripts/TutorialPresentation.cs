using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPresentation : MonoBehaviour
{
    public enum PresentationType
    {
        TurnLeft = 0,
        TurnRight,
        FlapBoth,
        Hover
    }

    public PresentationType Type;
    public float Duration = 1f;
    public float ScaleY = 1f;
    public float time = 0f;
    bool hoverLeft = false;

    Animator animator;
    Rigidbody Rigidbody;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        time += Time.deltaTime;

        bool flipped = false;
        if (time >= Duration)
        {
            time -= Duration;
            flipped = true;
        }

        if (Type == PresentationType.FlapBoth)
        {
            float x = time / Duration;

            x = (x + 0.5f) % 1f;

            float y = -4f * x * x + 4 * x;

            transform.localPosition = new Vector3(0f, y * ScaleY, 0f);

            if (flipped)
            {
                animator.SetTrigger("FlapR");
                animator.SetTrigger("FlapL");
                animator.SetTrigger("Bob");
            }
        }
        else if (Type == PresentationType.Hover)
        {
            float x = time / Duration;

            x = (x + 0.1f) % 1f;

            float y = -4f * x * x + 4 * x;
            transform.localPosition = new Vector3(0f, y * ScaleY, 0f);
            transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time) * 30f);

            if (flipped)
            {
                if (hoverLeft)
                {
                    animator.SetTrigger("FlapL");
                }
                else
                {
                    animator.SetTrigger("FlapR");
                }

                animator.SetTrigger("Bob");
                hoverLeft = !hoverLeft;
            }
        }
        else
        {
            float x = time / Duration;
            x = (x + 0.1f) % 1f;
            float y = -4f * x * x + 4 * x;
            transform.localPosition = new Vector3(0f, y * ScaleY, 0f);

            if (flipped)
            {
                if (Type == PresentationType.TurnLeft)
                {
                    animator.SetTrigger("FlapL");
                }
                else
                {
                    animator.SetTrigger("FlapR");
                }

                animator.SetTrigger("Bob");
                float direction = (Type == PresentationType.TurnLeft) ? 1f : -1f;
                Rigidbody.AddTorque(direction * transform.forward * 10f, ForceMode.Impulse);
            }
        }
    }
}
