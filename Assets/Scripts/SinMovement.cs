using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    public float StartDelay = 0f;
    public float MoveDuration = 2f;
    public float WaitTime = 0f;
    public Vector3 Movement = new Vector3(0, 0, 0);

    float time = 0f;
    Vector3 StartPosition;
    Vector3 EndPosition;
    
    void Start ()
    {
        StartPosition = transform.localPosition;
        EndPosition = StartPosition + Movement;

        time = -StartDelay;
    }
	
	void Update ()
    {
        time += Time.deltaTime;
        
        if (time >= 0f && time <= MoveDuration)
        {
            float ratio = time / MoveDuration;
            float value = Mathf.Sin(Mathf.PI * -0.5f + Mathf.PI * ratio) * 0.5f + 0.5f;
            transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, value);
        }
        else if (time > MoveDuration + WaitTime && time <= 2f * MoveDuration + WaitTime)
        {
            float ratio = (time - MoveDuration - WaitTime) / MoveDuration;
            float value = Mathf.Sin(Mathf.PI * -0.5f + Mathf.PI * ratio) * 0.5f + 0.5f;
            transform.localPosition = Vector3.Lerp(EndPosition, StartPosition, value);
        }
        else if (time > 2f * MoveDuration + 2f * WaitTime)
        {
            time -= 2f * MoveDuration + 2f * WaitTime;
        }
    }
}
