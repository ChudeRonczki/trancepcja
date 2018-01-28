using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Hud : MonoBehaviour
{
    public GameObject finishRoot;
    public GameObject[] finishPages;
    int currentFinishPage = 0;

    public bool LastPageShown
    {
        get
        {
            return currentFinishPage >= finishPages.Length - 1;
        }
    }

    private void Start()
    {
        Refresh();
        Game.Instance.StateChanged += Refresh;
        finishRoot.SetActive(false);

		GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Refresh()
    {
        if (Game.Instance.Finished && !finishRoot.activeSelf)
        {
            finishRoot.SetActive(true);
            RefreshFinish();
        }
    }

    private void RefreshFinish()
    {
        for (int i = 0; i < finishPages.Length; ++i)
            finishPages[i].SetActive(i == currentFinishPage);
    }

    internal void Proceed()
    {
        ++currentFinishPage;
        RefreshFinish();
    }
}
