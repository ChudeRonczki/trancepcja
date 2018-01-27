using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranContainer : MonoBehaviour {

    public float StartScale = 0.05f;
    public ParticleSystem StateChangeParticles;

    float ScaleStep;

    Vector3 currentScale;
    float targetScaleY;

    void Start ()
    {
        ScaleStep = (1f - StartScale) / Game.Instance.pointsTarget;

        currentScale = new Vector3(1f, StartScale, 1f);
        targetScaleY = StartScale;

        Game.Instance.StateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
        targetScaleY = Mathf.Min(StartScale + Game.Instance.TotalPointsCollected * ScaleStep, 1f);
        //StateChangeParticles.Play();
    }

    private void Update()
    {
        if (currentScale.y < targetScaleY)
        {
            float prevScaleY = currentScale.y;

            currentScale.y = Mathf.Lerp(currentScale.y, targetScaleY, 0.04f);

            if (prevScaleY % 0.25f > currentScale.y % 0.25f)
            {
                if (StateChangeParticles) StateChangeParticles.Play();
            }

            if (targetScaleY - currentScale.y < 0.01f)
            {
                currentScale.y = targetScaleY;
            }

            transform.localScale = currentScale;
        }
    }
}
