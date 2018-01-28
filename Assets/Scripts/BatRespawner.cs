using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatRespawner : MonoBehaviour
{
    public float respawnTime = 3f;
    public Image timeToRespawnImage;
    public Sprite[] timeToRespawnSprites;

    BatController controller;
    BatState state;
    Rigidbody body;
    Vector3 initialPosition;

    public AudioClip DeathSound;

    private void Awake()
    {
        controller = GetComponent<BatController>();
        state = GetComponent<BatState>();
        body = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        timeToRespawnImage.enabled = false;
    }

    public void Kill()
    {
        state.LoseTran();
        StartCoroutine(Respawn());

        AudioManager.Play(DeathSound, 1.3f, 1.6f);
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

        timeToRespawnImage.enabled = true;
        float timeLeft = respawnTime;
        timeToRespawnImage.sprite = timeToRespawnSprites[Mathf.Clamp(Mathf.CeilToInt(timeLeft) - 1,
            0, timeToRespawnSprites.Length)];

        while (timeLeft > 0f)
        {
            float waitTime = timeLeft - Mathf.Floor(timeLeft) + .01f;
            yield return new WaitForSeconds(waitTime);
            timeLeft -= waitTime;
            timeToRespawnImage.sprite = timeToRespawnSprites[Mathf.Clamp(Mathf.CeilToInt(timeLeft) - 1,
                0, timeToRespawnSprites.Length)];
        }

        timeToRespawnImage.enabled = false;

        controller.enabled = true;

        body.useGravity = body.detectCollisions = true;
    }
}
