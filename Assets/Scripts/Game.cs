using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public const int WallsLayer = 9;
    public const int CollectiblesLayer = 11;

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

    internal void GivePoints(Tran tran)
    {
        if (Finished || tran.lastOwner == -1)
            return;

        pointsCollected[tran.lastOwner] += Mathf.Min(tran.pointsWorth, pointsTarget - TotalPointsCollected);
        if (Finished)
            pointsCollected[tran.lastOwner] += lastDropAward;

        Destroy(tran.gameObject);

        if (StateChanged != null)
            StateChanged();
    }
}
