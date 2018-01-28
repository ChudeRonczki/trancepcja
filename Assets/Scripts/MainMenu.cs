using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text[] items;
    public Color inactiveColor;
    public Color activeColor;

    public Match matchPrefab;

    int[] mapsToPlay = { 1, 3, 5 };

    int activeId = 1;
    bool upLocked = false;
    bool downLocked = false;

    private void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < items.Length; ++i)
        {
            items[i].color = i == activeId
                ? activeColor
                : inactiveColor;

            items[i].GetComponent<Shadow>().enabled = (i == activeId);
        }
    }

    private void Update()
    {
        if (upLocked && Input.GetAxis("Vertical") < .25f)
            upLocked = false;

        if (downLocked && Input.GetAxis("Vertical") > -.25f)
            downLocked = false;

        if (!upLocked && Input.GetAxis("Vertical") > .8f)
        {
            upLocked = true;
            activeId = Mathf.Max(activeId - 1, 0);
            Refresh();
        }

        if (!downLocked && Input.GetAxis("Vertical") < -.8f)
        {
            downLocked = true;
            activeId = Mathf.Min(activeId + 1, items.Length - 1);
            Refresh();
        }

        if (Input.GetButtonDown("Fire1"))
            Proceed();
    }

    private void Proceed()
    {
        if (activeId == 3)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
        }
        else
        {
            var match = Instantiate<Match>(matchPrefab);
            match.mapsToPlay = mapsToPlay[activeId];
            SceneManager.LoadScene("Lobby");
        }
    }
}
