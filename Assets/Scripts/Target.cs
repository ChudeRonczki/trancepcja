using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField]
    ParticleSystem TranSplooshParticles;

    private void OnTriggerEnter(Collider other)
    {
        var tran = other.GetComponent<Tran>();
        if (tran)
        {
            Game.Instance.GivePoints(tran);

            if (TranSplooshParticles != null)
            {
                Instantiate(TranSplooshParticles, tran.transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
            }
        }
    }

}
