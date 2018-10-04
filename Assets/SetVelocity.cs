using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{
	public Vector3 velocity;
	private bool splashed = false;

	float splashTimer = 0;

	void OnEnable ()
	{
		var rb = GetComponent<Rigidbody>();
		if (rb)
			rb.velocity = velocity;
		splashed = false;
		splashTimer = 0;
	}

	void OnTriggerEnter (Collider other)
	{
		if (splashed || splashTimer < 1)
			return;

		var splash = PoolManager.manager.GetPooledObject("Splash");
		splash.transform.position = transform.position;

		splashed = true;

		GameObject text = PoolManager.manager.GetPooledObject("Terrible");
		text.transform.position = transform.position + new Vector3(0, 1.25f, 0);

		var retry = PoolManager.manager.GetPooledObject("Retry");
		retry.transform.position = new Vector3(0, 1.06f, -2.42f);
	}

	void Update ()
	{
		splashTimer += Time.deltaTime;
	}
}
