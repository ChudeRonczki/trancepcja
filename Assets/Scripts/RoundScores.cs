using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundScores : MonoBehaviour
{
    public Text winnerLabel;
    public Text[] playerScoresLabels;

    private void OnEnable()
    {
        if (Game.Instance == null)
            return;

        if (Game.Instance.Winner == 0)
            winnerLabel.text = "Round for Player 1";
        else if (Game.Instance.Winner == 1)
            winnerLabel.text = "Round for Player 2";
        else
            winnerLabel.text = "Round ends with a draw";

        for (int i = 0; i < playerScoresLabels.Length; ++i)
        {
            playerScoresLabels[i].text = string.Format("Player {0}: {1}", i + 1,
                Game.Instance.pointsCollected[i]);
        }
    }
}
