using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lobby : MonoBehaviour
{
    public BatState[] bats;
    public Match matchPrefab;

    public bool AllReady
    {
        get
        {
            foreach (var bat in bats)
            {
                if (bat)
                    return false;
            }

            return true;
        }
    }

    private void Start()
    {
        if (Match.Instance == null)
            Instantiate(matchPrefab);
    }

    void Update ()
    {
		if (AllReady)
        {
            Match.Instance.Proceed();
        }
	}
}
