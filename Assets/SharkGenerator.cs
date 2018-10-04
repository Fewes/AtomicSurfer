using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SharkGenerator : MonoBehaviour
{
	public GameObject m_Kraken;
	public float m_SharkFrequency = 1.5f;

	private Transform m_SharkStart;
    private Transform m_WaveStart2;
    private Transform m_SharkEnd;
    private Transform m_WaveEnd2;
    private float m_WaveTimer;
	public bool emissionEnabled = true;
	public bool m_BossActive = false;

	// Use this for initialization
	void Start ()
	{
        m_SharkStart = transform.Find("SharkStart");
        m_WaveStart2 = transform.Find("WavesStart2");
        m_SharkEnd = transform.Find("SharkEnd");
        m_WaveEnd2 = transform.Find("WavesEnd2");
    }

	void SpawnShark ()
	{
		if (PoolManager.manager.GetActiveObjectCount("SharkEnemy") > 5)
			return;

        var shark = PoolManager.manager.GetPooledObject("SharkEnemy");
		float r = UnityEngine.Random.Range(0f, 1f); // random number 
        float r2 = UnityEngine.Random.Range(0f, 1f); // random number 
        float r3 = UnityEngine.Random.Range(0f, 1f); // random number 
        Vector3 pos = Vector3.Lerp(m_SharkStart.position, m_SharkEnd.position, r); // random position 
        Vector3 pos2 = Vector3.Lerp(m_WaveStart2.position, m_WaveEnd2.position, r2); // random position 
        Vector3 final_pos = Vector3.Lerp(pos, pos2, r3); // random position 
        Vector3 add = new Vector3(0.0f, 0.8f, 0.0f);
                                                         //  shark.GetComponent<Enemy>().m_WaterRoot = GameObject.Find("Water_Root").transform;
        shark.transform.position = final_pos + add;
		m_WaveTimer = m_SharkFrequency;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var player = Object.FindObjectOfType<Player>();

		if (!player && Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		m_WaveTimer -= Time.deltaTime;

		if (m_WaveTimer < 0 && emissionEnabled)
            SpawnShark();

		if (player && player.GetScore() >= 50000)
		{
			emissionEnabled = false;
		}

		if (!emissionEnabled && !m_BossActive && PoolManager.manager.GetActiveObjectCount("SharkEnemy") < 1)
		{
			m_Kraken.SetActive(true);
			m_BossActive = true;
		}
	}
}
