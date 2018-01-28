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

    string[] shuffledLevels;
    int shuffledLevelId = -1;

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
        shuffledLevels = levels.Skip(1).OrderBy(level => UnityEngine.Random.value).ToArray();
    }

    internal void HandleGameFinished()
    {
        ++mapsPlayed;
    }

    internal void Proceed()
    {
        int lastIndex = Array.IndexOf(levels, SceneManager.GetActiveScene().name);
        string nextLevel;
        if (lastIndex == -1)
            nextLevel = levels[0];
        else
        {
            nextLevel = shuffledLevels[++shuffledLevelId];
        }

        if (!Finished)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextLevel);
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
