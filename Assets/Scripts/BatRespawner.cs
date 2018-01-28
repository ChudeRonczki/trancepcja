using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatRespawner : MonoBehaviour
{
    public float respawnTime = 3f;
    public Text timeToRespawnLabel;

    BatController controller;
    BatState state;
    Rigidbody body;
    Vector3 initialPosition;

    public AudioSource DeathSound;

    private void Awake()
    {
        controller = GetComponent<BatController>();
        state = GetComponent<BatState>();
        body = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        timeToRespawnLabel.enabled = false;
    }

    public void Kill()
    {
        state.LoseTran();
        StartCoroutine(Respawn());

        if (DeathSound)
        {
            DeathSound.pitch = UnityEngine.Random.value * 0.2f + 1.4f;
            DeathSound.Play();
        }
    }

    private IEnumerator Respawn()
    {
        controller.enabled = false;
        controller.ResetInput();

        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.useGravity = body.detectCollisions = false;

        timeToRespawnLabel.enabled = true;
        float timeLeft = respawnTime;
        timeToRespawnLabel.text = Mathf.CeilToInt(timeLeft).ToString();

        while (timeLeft > 0f)
        {
            float waitTime = timeLeft - Mathf.Floor(timeLeft) + .01f;
            yield return new WaitForSeconds(waitTime);
            timeLeft -= waitTime;
            timeToRespawnLabel.text = Mathf.CeilToInt(timeLeft).ToString();
        }

        timeToRespawnLabel.enabled = false;

        controller.enabled = true;

        body.useGravity = body.detectCollisions = true;
    }
}
