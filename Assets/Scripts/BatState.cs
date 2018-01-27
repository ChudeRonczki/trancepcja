using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BatState : MonoBehaviour
{
    public int ownerId = 0;
    public float ignoreTranTriggerMinTime = .25f;
    public float dropSpreadImpulse = .5f;
    public Transform tranContainer;
    public float containerScalePerPoint = .1f;

    public int CarriedPoints
    {
        get
        {
            return carriedTran.Sum(tran => tran.pointsWorth);
        }
    }
    public List<Tran> carriedTran = new List<Tran>();
    public bool ignoreTranTrigger = false;


    float lastDropTimestamp;

    private void Start()
    {
        RefreshContainer();
    }

    public void RefreshContainer()
    {
        tranContainer.localScale = new Vector3(CarriedPoints * containerScalePerPoint,
            CarriedPoints * containerScalePerPoint, CarriedPoints * containerScalePerPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        var tran = other.GetComponent<Tran>();
        if (tran && !ignoreTranTrigger)
        {
            carriedTran.Add(tran);
            tran.body.velocity = Vector3.zero;
            tran.body.useGravity = true;
            tran.gameObject.SetActive(false);
            tran.lastOwner = ownerId;
            RefreshContainer();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Drop" + (ownerId + 1)))
        {
            DropTran();
            lastDropTimestamp = Time.timeSinceLevelLoad;
            ignoreTranTrigger = true;
        }
        else if (Input.GetButtonUp("Drop" + (ownerId + 1)))
        {
            if (Time.timeSinceLevelLoad - lastDropTimestamp >= ignoreTranTriggerMinTime)
                ignoreTranTrigger = false;
            else
                StartCoroutine(WaitAndStopIgnoringTranTrigger());
        }

    }

    private IEnumerator WaitAndStopIgnoringTranTrigger()
    {
        yield return new WaitForSeconds(ignoreTranTriggerMinTime - Time.timeSinceLevelLoad + lastDropTimestamp);
        if (!Input.GetButton("Drop" + (ownerId + 1)))
            ignoreTranTrigger = false;
    }

    private void DropTran()
    {
        for (int i = 0; i < carriedTran.Count; ++i)
        {
            carriedTran[i].transform.position = transform.position;
            carriedTran[i].gameObject.SetActive(true);
            carriedTran[i].body.AddForce(
                new Vector3(dropSpreadImpulse * ((float)i - carriedTran.Count / 2f), 0f),
                ForceMode.Impulse);
        }

        carriedTran.Clear();
        RefreshContainer();
    }

    public void LoseTran()
    {
        carriedTran.ForEach(tran => Destroy(tran.gameObject));
        carriedTran.Clear();
        RefreshContainer();
    }
}
