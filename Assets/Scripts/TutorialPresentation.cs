using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPresentation : MonoBehaviour
{
    public enum PresentationType
    {
        Turn,
        FlapBoth
    }

    public PresentationType Type;
    public float Duration = 1f;
    public float SpriteDuration = 0.5f;
    public float ScaleY = 1f;
    public float time = 0f;
    bool turnLeft = false;
    bool firstTurn = true;

    Animator animator;
    Rigidbody Rigidbody;

    public SpriteRenderer[] Sprites = new SpriteRenderer[2];

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

            Sprites[0].enabled = time < SpriteDuration;
            Sprites[1].enabled = time < SpriteDuration;
        }
        else
        {
            float x = time / Duration;
            x = (x + 0.1f) % 1f;
            float y = -4f * x * x + 4 * x;
            transform.localPosition = new Vector3(0f, y * ScaleY, 0f);
            
            if (flipped)
            {
                if (turnLeft)
                {
                    animator.SetTrigger("FlapL");
                }
                else
                {
                    animator.SetTrigger("FlapR");
                }

                animator.SetTrigger("Bob");
                float direction = (turnLeft) ? 1f : -1f;
                Rigidbody.AddTorque(direction * transform.forward * 10f, ForceMode.Impulse);

                if (!firstTurn)
                {
                    turnLeft = !turnLeft;
                }

                firstTurn = !firstTurn;
            }

            if ((turnLeft && firstTurn) || (!turnLeft && !firstTurn))
            {
                Sprites[0].enabled = time < SpriteDuration;
                Sprites[1].enabled = false;
            }
            else
            {
                Sprites[0].enabled = false;
                Sprites[1].enabled = time < SpriteDuration;
            }
        }
    }
}
