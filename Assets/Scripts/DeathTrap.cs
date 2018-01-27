using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    public bool destroyTran = true;
    public ParticleSystem particlesOnDeath;
    public Vector3 ParticlesOffset = Vector3.zero;

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
        }
            

        var tran = other.GetComponent<Tran>();
        if (tran && destroyTran)
            Destroy(tran.gameObject);
    }
}
