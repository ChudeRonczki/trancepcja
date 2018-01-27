using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Hud : MonoBehaviour
{
    public Text progressLabel;
    public Text winnerLabel;

    private void Start()
    {
        Refresh();
        Game.Instance.StateChanged += Refresh;
    }

    private void Refresh()
    {
        progressLabel.text = String.Format("{0}/{1}", Game.Instance.TotalPointsCollected,
            Game.Instance.pointsTarget);

        if (Game.Instance.Finished)
        {
            if (Game.Instance.pointsCollected[0] > Game.Instance.pointsCollected[1])
                winnerLabel.text = "Player 1 Wins";
            else if (Game.Instance.pointsCollected[0] < Game.Instance.pointsCollected[1])
                winnerLabel.text = "Player 2 Wins";
            else
                winnerLabel.text = "Even Game";
        }
        else
            winnerLabel.text = "";
    }
}
