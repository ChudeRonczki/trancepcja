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
            int maxIndex = 0;
            for (int i = 1; i < Game.Instance.pointsCollected.Length; ++i)
            {
                if (Game.Instance.pointsCollected[i] > Game.Instance.pointsCollected[maxIndex])
                    maxIndex = i;
            }
            winnerLabel.text = String.Format("Player {0} Wins", maxIndex + 1);
        }
        else
            winnerLabel.text = "";
    }
}
