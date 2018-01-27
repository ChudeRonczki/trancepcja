using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatSacrificeAltar : MonoBehaviour
{
    public Text readyLabel;
    bool isReady = false;


    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatState>();
        if (bat && !isReady)
        {
            isReady = true;
            readyLabel.text = "Ready";
            Destroy(bat.gameObject);
        }
    }
}
