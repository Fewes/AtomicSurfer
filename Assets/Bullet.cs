using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// Public
	public float speed = 1;
	public bool enemyBullet;
	public float damage;

	// Private
	float life = 2f;
	
	// Use this for initialization
	void Start ()
	{
		
	}

	void OnEnable ()
	{
		life = 2f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		life -= Time.deltaTime;

		if (life < 0)
			gameObject.SetActive(false);

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
