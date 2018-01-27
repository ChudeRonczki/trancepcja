using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public int[] pointsCollected = { 0, 0 };
    public int pointsTarget = 10;
    public int lastDropAward = 2;

    public event Action StateChanged;

    public int TotalPointsCollected
    {
        get
        {
            return pointsCollected.Sum();
        }
    }

    public bool Finished
    {
        get
        {
            return TotalPointsCollected >= pointsTarget;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    internal void GivePoints(BatState bat)
    {
        if (Finished)
            return;

        pointsCollected[bat.ownerId] += Mathf.Min(bat.carriedPoints, pointsTarget - TotalPointsCollected);
        bat.carriedPoints = 0;
        if (Finished)
            pointsCollected[bat.ownerId] += lastDropAward;

        if (StateChanged != null)
            StateChanged();
    }
}
