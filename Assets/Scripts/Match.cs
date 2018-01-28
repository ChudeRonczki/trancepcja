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

    public bool[] inverseDirections = { true, true };

    public bool Finished
    {
        get
        {
            return mapsPlayed >= mapsToPlay;
        }
    }

    public int WinnerId
    {
        get
        {
            if (!Finished)
                return -1;
            else if (gamesWon[0] > gamesWon[1])
                return 0;
            else if (gamesWon[1] > gamesWon[0])
                return 1;
            else
                return -1;
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
            lastIndex = UnityEngine.Random.Range(-1, 1); // Start with Level1 or Level2

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
        match.inverseDirections = inverseDirections;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby");
        Destroy(gameObject);
    }
}
