using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	// Public

	// Private
	private Quaternion m_StoredRot;

	// Use this for initialization
	void Start ()
	{
		m_StoredRot = transform.rotation;
	}

	public void Kick()
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Random.rotation, 0.003f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, m_StoredRot, Time.deltaTime / 0.1f);
	}
}
