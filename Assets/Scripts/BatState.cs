using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatState : MonoBehaviour
{
    public int carriedPoints = 0;

    private void OnTriggerEnter(Collider other)
    {
        var tran = other.GetComponent<Tran>();
        if (tran)
        {
            carriedPoints += tran.pointsWorth;
            Destroy(tran.gameObject);
        }
    }
}
