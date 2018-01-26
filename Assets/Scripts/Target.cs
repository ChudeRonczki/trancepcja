using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public int pointsCollected = 0;
    public Text pointsLabel; 

    private void OnTriggerEnter(Collider other)
    {
        var bat = other.GetComponent<BatState>();
        if (bat)
        {
            pointsCollected += bat.carriedPoints;
            bat.carriedPoints = 0;
            pointsLabel.text = pointsCollected.ToString();
        }
    }

}
