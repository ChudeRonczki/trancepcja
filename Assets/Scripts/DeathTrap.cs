using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatRespawner>();
        if (bat)
            bat.Kill();
    }
}
