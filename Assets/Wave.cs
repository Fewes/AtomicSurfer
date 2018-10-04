using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
	public Transform m_WaterRoot;
	public float m_WaveSpeed = 1f;
	public bool jumped = false;
	// Use this for initialization
	void Start ()
	{
		
	}

	void OnEnable ()
	{
		jumped = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position -= m_WaterRoot.forward * Time.deltaTime * m_WaveSpeed;
	}
}
