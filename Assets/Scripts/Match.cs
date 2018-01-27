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

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    internal void Proceed()
    {
        SceneManager.LoadScene(levels[(Array.IndexOf(levels, SceneManager.GetActiveScene().name) + 1) % levels.Length]);
    }
}
