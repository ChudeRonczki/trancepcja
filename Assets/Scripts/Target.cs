using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatState>();
        if (bat)
        {
            bat.LoseTran();
            return;
        }

        var tran = other.GetComponent<Tran>();
        if (tran)
        {
            Game.Instance.GivePoints(tran);
            return;
        }
    }

}
