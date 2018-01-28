using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScores : MonoBehaviour
{
    public Text progressLabel;
    public Text[] playerTotalLabels;
    public Text restartHintLabel;

    private void OnEnable()
    {
        if (Match.Instance == null)
            return;

        if (Match.Instance.Finished)
        {
            switch (Match.Instance.WinnerId)
            {
                case -1:
                    progressLabel.text = "DRAW";
                    break;
                default:
                    progressLabel.text = string.Format("PLAYER {0} WON!", Match.Instance.WinnerId);
                    break;
            }
            restartHintLabel.gameObject.SetActive(true);
        }
        else
        {
            progressLabel.text = string.Format("GAME {0}/{1}",
                Match.Instance.mapsPlayed, Match.Instance.mapsToPlay);
            restartHintLabel.gameObject.SetActive(false);
        }

        for (int i = 0; i < playerTotalLabels.Length; ++i)
        {
            playerTotalLabels[i].text = string.Format("PLAYER {0}: {1}", i + 1,
                Match.Instance.gamesWon[i]);
        }
    }
}
