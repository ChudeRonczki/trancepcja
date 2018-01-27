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

    public int pointsTarget = 10;
    public int lastDropAward = 2;

    public Match matchPrefab;

    public int[] pointsCollected = { 0, 0 };

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

    public int Winner
    {
        get
        {
            if (Finished)
            {
                if (pointsCollected[0] > pointsCollected[1])
                    return 0;
                if (pointsCollected[1] > pointsCollected[0])
                    return 1;
            }

            return -1;
        }
    }

    private void Awake()
    {
        Instance = this;
        if (Match.Instance == null)
            Instantiate(matchPrefab);
    }

    internal void GivePoints(Tran tran)
    {
        if (Finished || tran.lastOwner == -1)
            return;

        pointsCollected[tran.lastOwner] += Mathf.Min(tran.pointsWorth, pointsTarget - TotalPointsCollected);
        if (Finished)
        {
            pointsCollected[tran.lastOwner] += lastDropAward;
            if (Winner != -1)
                ++Match.Instance.gamesWon[Winner];
        }

        Destroy(tran.gameObject);

        if (StateChanged != null)
            StateChanged();
    }

    private void Update()
    {
        if (Finished && Input.GetButtonDown("Proceed"))
            Match.Instance.Proceed();
    }
}
