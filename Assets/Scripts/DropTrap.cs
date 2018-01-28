using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrap : MonoBehaviour
{
    public ParticleSystem particlesOnTouch;
    public Vector3 ParticlesOffset = Vector3.zero;
    public AudioClip TouchClip;
    public float PitchMin = 0.9f;
    public float PitchMax = 1f;

    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatState>();
        if (bat)
        {
            if (bat.DropTran())
            {
                if (particlesOnTouch)
                {
                    Instantiate(particlesOnTouch, bat.transform.position + ParticlesOffset, Quaternion.identity);
                }

                AudioManager.Play(TouchClip, PitchMin, PitchMax);
            }
        }
    }
}
