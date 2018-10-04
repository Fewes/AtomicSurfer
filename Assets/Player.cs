using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	// Public
	public Transform m_WaterRoot;
	public float m_MovementSpeedForwards = 1;
	public float m_MovementSpeedUpwards = 1;
	public float m_MovementSpeedForwardsMax = 1;
	public float m_MovementSpeedUpwardsMax = 1;
	public float m_Decceleration = 0.5f;
	public AudioClip m_JumpSound;

	// Private
	private Animator m_Animator;
	private float m_MovementForwards = 0;
	private float m_MovementUpwards = 0;
	private float m_Health = 100;
	private bool m_IsJumping;

	private int m_Score;

	// Use this for initialization
	void Start ()
	{
		m_Animator = transform.Find("Visual").Find("Sprite").GetComponent<Animator>();
	}

	public bool IsAlive ()
	{
		return m_Health > 0;
	}

	public void AddScore(int s)
	{
		m_Score += s;
		Object.FindObjectOfType<Canvas>().transform.Find("Score").GetComponent<Text>().text = "SCORE: " + m_Score.ToString();
	}

	public int GetScore()
	{
		return m_Score;
	}

	void OnTriggerEnter (Collider other)
	{
		var bullet = other.gameObject.GetComponent<Bullet>();
		if (!bullet || !bullet.enemyBullet || m_IsJumping)
			return;

		var go = PoolManager.manager.GetPooledObject("BloodSpurt");
		go.transform.position = other.transform.position;
		go.transform.rotation = other.transform.rotation;
		
		// Disable bullet
		bullet.gameObject.SetActive(false);

		transform.Find("Visual").Find("Sprite").GetComponent<Flasher>().Flash();

		m_Animator.SetTrigger("Hurt");

		m_Health -= bullet.damage;
		UpdateHealthBar();
		if (m_Health < 0)
			Die();
	}

	public void Die ()
	{
		//Object.FindObjectOfType<PlayerView>().SetIntensity(1);
		var deadPlayer = PoolManager.manager.GetPooledObject("SurferDead");
		deadPlayer.transform.position = transform.position;
		gameObject.SetActive(false);

		// Hide UI except score
		Transform t = Object.FindObjectOfType<Canvas>().transform;
		foreach (Transform child in t)
		{
			if (child.gameObject.name != "Score")
				child.gameObject.SetActive(false);
		}
	}

	public void UpdateHealthBar()
	{
		var scale = Object.FindObjectOfType<Canvas>().transform.Find("HealthBar").GetComponent<RectTransform>().localScale;
		scale.x = Mathf.Max(m_Health, 0) / 25;
		Object.FindObjectOfType<Canvas>().transform.Find("HealthBar").GetComponent<RectTransform>().localScale = scale;
	}

	public IEnumerator Jump ()
	{
		m_IsJumping = true;

		m_Animator.SetTrigger("Flip");
		transform.Find("Foam").GetComponent<ParticleSystem>().enableEmission = false;

		m_Health = Mathf.Min(m_Health + 10, 100);
		UpdateHealthBar();

		transform.Find("Visual").GetComponent<Animator>().SetTrigger("Jump");

		AudioSource.PlayClipAtPoint(m_JumpSound, transform.position);

		GameObject text = null;
		int r = UnityEngine.Random.Range(0, 5);
		if (r == 0)
			text = PoolManager.manager.GetPooledObject("Radical");
		else if (r == 1)
			text = PoolManager.manager.GetPooledObject("Sick");
		else if (r == 2)
			text = PoolManager.manager.GetPooledObject("Gnarly");
		else if (r == 3)
			text = PoolManager.manager.GetPooledObject("Awesome");
		else if (r == 4)
			text = PoolManager.manager.GetPooledObject("Dude");
		if (text)
			text.transform.position = transform.position + new Vector3(0, 1.25f, 0);

		yield return new WaitForSeconds(1);

		// Landing
		transform.Find("Foam").GetComponent<ParticleSystem>().enableEmission = true;
		m_IsJumping = false;

		AddScore(250);

		transform.Find("Splash").GetComponent<ParticleSystem>().Emit(50);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsAlive())
		{
			float m = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
			var screenPos = Camera.main.WorldToScreenPoint(transform.position);
			screenPos.x /= Camera.main.pixelWidth;
			screenPos.y /= Camera.main.pixelHeight;
			float playerWidth = 0.05f;
			float playerHeight = 0.1f;

			// Movement
			if (Input.GetKey(KeyCode.W))
			{
				m_MovementUpwards += m_MovementSpeedUpwards * Time.deltaTime;
				if (m_MovementUpwards > m_MovementSpeedUpwardsMax * m)
					m_MovementUpwards = m_MovementSpeedUpwardsMax * m;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				m_MovementUpwards -= m_MovementSpeedUpwards * Time.deltaTime;
				if (m_MovementUpwards < -m_MovementSpeedUpwardsMax * m)
					m_MovementUpwards = -m_MovementSpeedUpwardsMax * m;
			}
			else
			{
				if (m_MovementUpwards > 0)
					m_MovementUpwards = Mathf.Max(m_MovementUpwards - Time.deltaTime * m_Decceleration, 0);
				else
					m_MovementUpwards = Mathf.Min(m_MovementUpwards + Time.deltaTime * m_Decceleration, 0);
			}


			if (Input.GetKey(KeyCode.D))
			{
				m_MovementForwards += m_MovementSpeedForwards * Time.deltaTime;
				if (m_MovementForwards > m_MovementSpeedForwardsMax * m)
					m_MovementForwards = m_MovementSpeedForwardsMax * m;
				m_Animator.SetFloat("Forward", 1);
			}
			else if (Input.GetKey(KeyCode.A))
			{
				m_MovementForwards -= m_MovementSpeedForwards * Time.deltaTime;
				if (m_MovementForwards < -m_MovementSpeedForwardsMax * m)
					m_MovementForwards = -m_MovementSpeedForwardsMax * m;
				m_Animator.SetFloat("Forward", -1);
			}
			else
			{
				if (m_MovementForwards > 0)
					m_MovementForwards = Mathf.Max(m_MovementForwards - Time.deltaTime * m_Decceleration, 0);
				else
					m_MovementForwards = Mathf.Min(m_MovementForwards + Time.deltaTime * m_Decceleration, 0);
				m_Animator.SetFloat("Forward", 0);
			}

			bool moveHorizontal = true;
			if (m_MovementForwards < 0 && screenPos.x < playerWidth)
				moveHorizontal = false;
			if (m_MovementForwards > 0 && screenPos.x > 1 - playerWidth)
				moveHorizontal = false;

			bool moveVertical = true;
			if (m_MovementUpwards < 0 && screenPos.y < playerHeight)
				moveVertical = false;
			if (m_MovementUpwards > 0 && screenPos.y > .75f - playerHeight)
				moveVertical = false;

			if (moveHorizontal)
				transform.position += m_WaterRoot.forward * m_MovementForwards;
			else
				m_MovementForwards = 0;
			if (moveVertical)
				transform.position += m_WaterRoot.up * m_MovementUpwards;
			else
				m_MovementUpwards = 0;

			if (!m_IsJumping)
			{
				// Check waves
				var waves = Object.FindObjectsOfType<Wave>();
				foreach (var wave in waves)
				{
					if (!wave.jumped && Vector3.Distance(transform.position, wave.transform.position) < 2)
					{
						wave.jumped = true;
						StartCoroutine(Jump());
					}
				}
			}
		}

	}
}
