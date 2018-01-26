using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Target target;
    public Text pointsLabel;

    private void Start()
    {
        target.StateChanged += Refresh;
    }

    private void Refresh()
    {
        pointsLabel.text = target.pointsCollected.ToString();
    }
}
