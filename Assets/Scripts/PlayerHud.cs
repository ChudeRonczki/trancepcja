using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public int ownerId;
    public Text pointsLabel;

    private void Start()
    {
        Game.Instance.StateChanged += Refresh;
    }

    private void Refresh()
    {
        pointsLabel.text = Game.Instance.pointsCollected[ownerId].ToString();
    }
}
