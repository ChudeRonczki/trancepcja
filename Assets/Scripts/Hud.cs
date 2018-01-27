using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Hud : MonoBehaviour
{
    public Text progressLabel;
    public GameObject finishRoot;
    public Text winnerLabel;
    public Text[] playerTotalLabels;

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
            finishRoot.SetActive(true);

            if (Game.Instance.Winner == 0)
                winnerLabel.text = "Player 1 Wins";
            else if (Game.Instance.Winner == 1)
                winnerLabel.text = "Player 2 Wins";
            else
                winnerLabel.text = "Even Game";

            for (int i = 0; i < playerTotalLabels.Length; ++i)
            {
                playerTotalLabels[i].text = String.Format("Player {0}: {1}", i + 1,
                    Match.Instance.gamesWon[i]);
            }
        }
        else
            finishRoot.SetActive(false);
    }
}
