using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    public bool destroyTran = true;

    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatRespawner>();
        if (bat)
            bat.Kill();

        var tran = other.GetComponent<Tran>();
        if (tran && destroyTran)
            Destroy(tran.gameObject);
    }
}
