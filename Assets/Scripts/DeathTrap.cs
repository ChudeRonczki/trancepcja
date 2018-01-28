using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    public bool destroyTran = true;
    public ParticleSystem particlesOnDeath;
    public Vector3 ParticlesOffset = Vector3.zero;
    public AudioClip DeathClip;
    public float PitchMin = 0.9f;
    public float PitchMax = 1f;

    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatRespawner>();
        if (bat)
        {
            if (particlesOnDeath)
            {
                Instantiate(particlesOnDeath, bat.transform.position + ParticlesOffset, Quaternion.identity);
            }

            bat.Kill();
            AudioManager.Play(DeathClip, PitchMin, PitchMax);
        }
            

        var tran = other.GetComponent<Tran>();
        if (tran && destroyTran)
            Destroy(tran.gameObject);
    }
}
