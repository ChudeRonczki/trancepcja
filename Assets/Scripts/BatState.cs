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
    public float containerStartScale = 1f;

    public int CarriedPoints
    {
        get
        {
            return carriedTran.Sum(tran => tran.pointsWorth);
        }
    }
    public List<Tran> carriedTran = new List<Tran>();
    public bool ignoreTranTrigger = false;
    public ParticleSystem tranCollectParticles;


    ParticleSystem tranParticleSystem;
    float lastDropTimestamp;
    Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        tranParticleSystem = tranContainer.GetComponentInChildren<ParticleSystem>();
        RefreshContainer();
    }

    public void RefreshContainer()
    {
        if (CarriedPoints > 0f)
        {
            float scale = containerStartScale + CarriedPoints * containerScalePerPoint;
            tranContainer.localScale = new Vector3(scale, scale, scale);
            if (tranParticleSystem)
            {
                var emi = tranParticleSystem.emission;
                emi.rateOverTime = 1f + 0.5f * CarriedPoints;
                var shap = tranParticleSystem.shape;
                shap.radius = scale * 0.1f;
            }
        }
        else
        {
            tranContainer.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            if (tranParticleSystem)
            {
                var emi = tranParticleSystem.emission;
                emi.rateOverTime = 0;
            }
        }
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
            if (tranCollectParticles) Instantiate(tranCollectParticles, tran.transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Drop" + (ownerId + 1)))
        {
            DropTran();
        }
        else if (Input.GetButtonUp("Drop" + (ownerId + 1)))
        {
            if (Time.timeSinceLevelLoad - lastDropTimestamp >= ignoreTranTriggerMinTime)
                ignoreTranTrigger = false;
        }

    }

    private IEnumerator WaitAndStopIgnoringTranTrigger()
    {
        yield return new WaitForSeconds(ignoreTranTriggerMinTime);
        if (!Input.GetButton("Drop" + (ownerId + 1)))
            ignoreTranTrigger = false;
    }

    internal void DropTran()
    {
        for (int i = 0; i < carriedTran.Count; ++i)
        {
            carriedTran[i].transform.position = transform.position;
            carriedTran[i].Drop(new Vector3(dropSpreadImpulse * ((float)i - carriedTran.Count / 2f)
                + body.velocity.x, 0f));
        }

        carriedTran.Clear();
        RefreshContainer();
        lastDropTimestamp = Time.timeSinceLevelLoad;
        ignoreTranTrigger = true;
        StartCoroutine(WaitAndStopIgnoringTranTrigger());
    }

    public void LoseTran()
    {
        carriedTran.ForEach(tran => Destroy(tran.gameObject));
        carriedTran.Clear();
        RefreshContainer();
    }
}
