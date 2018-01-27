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
            out hitInfo, body.velocity.magnitude * Time.fixedDeltaTime, 1 << Game.WallsLayer))
        {
            if (Mathf.Abs(hitInfo.normal.x) > .8f)
            {
                body.AddForce(new Vector3(Mathf.Sign(hitInfo.normal.x) * .1f, 0f, 0f));
            }
            else
            {
                body.useGravity = false;
                body.position += hitInfo.distance * body.velocity.normalized;
                body.velocity = Vector3.zero;
            }
        }
    }
}
