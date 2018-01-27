using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Match : MonoBehaviour
{
    public static Match Instance;

    public string[] levels;

    public int[] gamesWon = { 0, 0 };

    public int mapsToPlay = 3;

    public int mapsPlayed = 0;

    public bool Finished
    {
        get
        {
            return mapsPlayed >= mapsToPlay;
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    internal void HandleGameFinished()
    {
        ++mapsPlayed;
    }

    internal void Proceed()
    {
        int lastIndex = Array.IndexOf(levels, SceneManager.GetActiveScene().name);
        if (lastIndex == -1)
            lastIndex = UnityEngine.Random.Range(0, levels.Length);

        if (!Finished)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(levels[(lastIndex + 1) % levels.Length]);
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
            Destroy(gameObject);
        }
    }

    internal void Restart(Match matchPrefab)
    {
        var match = Instantiate<Match>(matchPrefab);
        match.mapsToPlay = mapsToPlay;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby");
        Destroy(gameObject);
    }
}
