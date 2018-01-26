using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMovement : MonoBehaviour
{
    BatState bat;

    private void Start()
    {
        bat = GetComponent<BatState>();
    }

    void Update()
    {
        if (bat.ownerId == 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")) * 10f;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")) * 20f;
        }
	}
}
