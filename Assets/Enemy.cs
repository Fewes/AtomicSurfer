using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	// Public
	public float m_FireInterval;
	public AudioClip m_GunshotSound;
	public float m_MaxHealth = 100;
	public bool lastBoss = false;
	public GameObject winScreen;

	// Private
	private Transform m_Barrel;
	private float m_FireTimer;
    protected float m_Health;

    // Use this for initialization
    void Start ()
	{
        m_Health = m_MaxHealth;
		m_Barrel = transform.Find("Barrel");

		m_FireTimer = .25f;
    }

	void OnTriggerEnter (Collider other)
	{
        var bullet = other.gameObject.GetComponent<Bullet>();
		if (!bullet || bullet.enemyBullet)
			return;

		var go = PoolManager.manager.GetPooledObject("BloodSpurt");
		go.transform.position = other.transform.position;
		go.transform.rotation = other.transform.rotation;
		
		// Disable bullet
		bullet.gameObject.SetActive(false);

		GetComponent<Flasher>().Flash();

		m_Health -= bullet.damage;
		if (m_Health < 0)
			Die();
	}

	public bool IsAlive ()
	{
		return m_Health > 0;
	}

	public void Die ()
	{
		var go = PoolManager.manager.GetPooledObject("BloodExplosion");
		go.transform.position = transform.position;
		gameObject.SetActive(false);
		var player = Object.FindObjectOfType<Player>();
		if (player)
			Object.FindObjectOfType<Player>().AddScore(500);

		if (lastBoss)
		{
			// Hide UI except score
			Transform t = Object.FindObjectOfType<Canvas>().transform;
			foreach (Transform child in t)
			{
				if (child.gameObject.name != "Score")
					child.gameObject.SetActive(false);
			}
			winScreen.SetActive(true);
			if (player)
				player.gameObject.SetActive(false);
		}
	}

	void OnEnable ()
	{
        m_Health = m_MaxHealth;
		m_FireTimer = .25f;

		GetComponent<Flasher>().ResetFlash();
	}

	void Fire ()
	{
		// Find player
		var player = Object.FindObjectOfType<Player>();
		if (!player || !player.IsAlive())
			return;

		// Orient barrel
		Vector2 coord = Camera.main.WorldToScreenPoint(player.transform.position);

		var v1 = new Vector2(1, 0);
		var v2 = coord - new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		var rot = transform.localRotation.eulerAngles;
		if (coord.y > Input.mousePosition.y)
			rot.y = -Vector2.Angle(v1, v2) - 180;
		else
			rot.y = Vector2.Angle(v1, v2) + 180;
		m_Barrel.localRotation = Quaternion.Euler(rot);

		m_Barrel.rotation = Quaternion.LookRotation(player.transform.position - m_Barrel.transform.position);

		AudioSource.PlayClipAtPoint(m_GunshotSound, transform.position);

		var bullet = PoolManager.manager.GetPooledObject("EnemyBullet");
		bullet.transform.position = m_Barrel.transform.position;
		bullet.transform.rotation = m_Barrel.transform.rotation;

		m_FireTimer = m_FireInterval;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsAlive())
		{
			m_FireTimer -= Time.deltaTime;
			if (m_FireTimer < 0 && m_Barrel)
			{
				Fire();
			}
		}
	}
}
