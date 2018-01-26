using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public int ownerId = 0;
    public int pointsCollected = 0;

    public event Action StateChanged;

    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatState>();
        if (bat)
        {
            pointsCollected += bat.carriedPoints;
            bat.carriedPoints = 0;
            if (StateChanged != null)
                StateChanged();
        }
    }

}
