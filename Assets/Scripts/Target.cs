using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    ParticleSystem TranSplooshParticles;
    
    public AudioClip DropClip;
    public float PitchMin = 0.9f;
    public float PitchMax = 1f;

    float LastSoundTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        var tran = other.GetComponent<Tran>();
        if (tran)
        {
            Game.Instance.GivePoints(tran);

            if (LastSoundTime < Time.time - 0.1f)
            {
                AudioManager.Play(DropClip, PitchMin, PitchMax);
                LastSoundTime = Time.time;
            }

            if (TranSplooshParticles != null)
            {
                Instantiate(TranSplooshParticles, tran.transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
            }
        }
    }
}
