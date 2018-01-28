using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public const int WallsLayer = 9;
    public const int CollectiblesLayer = 11;

    public int pointsTarget = 10;
    public int lastDropAward = 2;

    public Match matchPrefab;

    public int[] pointsCollected = { 0, 0 };

    public GameObject countdownRoot;
    public Text countdownLabel;

    public event Action StateChanged;

    Hud hud;

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

        hud = FindObjectOfType<Hud>();
    }

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        Time.timeScale = 0f;
        countdownRoot.SetActive(true);
        for (int i = 3; i > 0; --i)
        {
            countdownLabel.text = i.ToString();
            for (float time = 1f; time > 0f; time -= Time.unscaledDeltaTime)
            {
                countdownLabel.rectTransform.localScale = new Vector3(time, time);
                yield return null;
            }
        }
        Time.timeScale = 1f;
        countdownRoot.SetActive(false);
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

            StartCoroutine(EaseTimeStop());
            Match.Instance.HandleGameFinished();
        }

        Destroy(tran.gameObject);

        if (StateChanged != null)
            StateChanged();
    }

    private IEnumerator EaseTimeStop()
    {
        for (float time = 1f; time > 0f; time -= Time.unscaledDeltaTime)
        {
            Time.timeScale = time;
            yield return null;
        }

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Finished && Input.GetButtonDown("Proceed"))
        {
            if (hud.LastPageShown)
                Match.Instance.Proceed();
            else
                hud.Proceed();
        }
        else if (Finished && hud.LastPageShown
            && Match.Instance.Finished && Input.GetButtonDown("Fire1"))
        {
            Match.Instance.Restart(matchPrefab);
        }
    }
}
