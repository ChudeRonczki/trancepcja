using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour
{
	[SerializeField]
	float m_min = 1;

	[SerializeField]
	float m_max = 2;

	// Update is called once per frame
	void Update()
	{
		transform.localScale = Vector3.one * Mathf.Lerp(m_min, m_max, Mathf.PingPong(Time.time, 1));
	}
}
