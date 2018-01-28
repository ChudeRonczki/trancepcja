using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tran : MonoBehaviour
{
    public int pointsWorth = 1;

    public Rigidbody body;
    public SphereCollider sphereCollider;

    public int lastOwner = -1;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        RaycastHit hitInfo;
        if (body.useGravity && Physics.SphereCast(body.position,
            sphereCollider.radius * transform.localScale.x, body.velocity,
            out hitInfo, body.velocity.magnitude * Time.fixedDeltaTime * 2f, 1 << Game.WallsLayer))
        {
            if (Mathf.Abs(hitInfo.normal.x) > .9f)
            {
                body.AddForce(new Vector3(Mathf.Sign(hitInfo.normal.x) * Mathf.Abs(body.velocity.x), 0f, 0f), ForceMode.Impulse);
            }
            else
            {
                body.useGravity = false;
                body.position += hitInfo.distance * body.velocity.normalized;
                body.velocity = Vector3.zero;
            }
        }
    }

    internal void Drop(Vector3 force)
    {
        gameObject.SetActive(true);
        RaycastHit hitInfo;
        if (Physics.Raycast(body.position, Vector3.down, out hitInfo, sphereCollider.radius, 1 << Game.WallsLayer))
        {
            if (Vector3.Dot(Vector3.up, hitInfo.normal) > .1f)
            {
                body.useGravity = false;
                return;
            }
        }

        body.AddForce(force, ForceMode.Impulse);
    }
}
