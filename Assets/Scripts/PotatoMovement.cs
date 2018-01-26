using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMovement : MonoBehaviour
{
	void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")) * 10f;
	}
}
