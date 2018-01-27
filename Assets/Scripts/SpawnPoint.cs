using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }

    public bool Empty
    {
        get
        {
            return !Physics.CheckSphere(transform.position, .5f, 1 << Game.CollectiblesLayer);
        }
    }
}
