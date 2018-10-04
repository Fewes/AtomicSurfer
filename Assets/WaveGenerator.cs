using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
	public float m_WaveFrequency = 5;

	private Transform m_WaveStart;
	private Transform m_WaveEnd;
	private float m_WaveTimer;

	// Use this for initialization
	void Start ()
	{
		m_WaveStart = transform.Find("WavesStart");
		m_WaveEnd = transform.Find("WavesEnd");
	}

	void SpawnWave ()
	{
		var wave = PoolManager.manager.GetPooledObject("Wave");
		float r = UnityEngine.Random.Range(0f, 1f);
		Vector3 pos = Vector3.Lerp(m_WaveStart.position, m_WaveEnd.position, r);
		wave.GetComponent<Wave>().m_WaterRoot = GameObject.Find("Water_Root").transform;
		wave.transform.position = pos;
		m_WaveTimer = m_WaveFrequency;
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_WaveTimer -= Time.deltaTime;
		if (m_WaveTimer < 0)
			SpawnWave();
	}
}
