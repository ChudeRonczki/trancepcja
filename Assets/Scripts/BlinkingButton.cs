using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingButton : MonoBehaviour
{
    public float Duration = 0.5f;
    public float VisibleTime = 0.1f;
    public SpriteRenderer Sprite;

    float time = 0f;

	void Update ()
    {
        time += Time.deltaTime;

        if (time >= Duration)
        {
            time -= Duration;
        }

        Sprite.enabled = time < VisibleTime;
    }
}
