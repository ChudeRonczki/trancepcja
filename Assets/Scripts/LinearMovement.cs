using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public float StartDelay = 0f;
    public float MoveDuration = 2f;
    public float WaitTime = 0f;
    public Vector3 Movement = new Vector3(0, 0, 0);

    float time = 0f;
    Vector3 StartPosition;
    Vector3 EndPosition;

    void Start()
    {
        StartPosition = transform.localPosition;
        EndPosition = StartPosition + Movement;

        time = -StartDelay;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 0f && time <= MoveDuration)
        {
            float ratio = time / MoveDuration;
            transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, ratio * ratio);
        }
        else if (time > MoveDuration + WaitTime)
        {
            time -= MoveDuration + WaitTime;
        }
    }
}
