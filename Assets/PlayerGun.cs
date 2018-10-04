using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
	// Public
	public GameObject bulletPrefab;
	public AudioClip gunshotSound;
	public AudioClip reloadSound;
	public AudioClip emptySound;
	public float fireInterval;
	public bool automatic;

	// Private
	private Transform m_Barrel;
	private ParticleSystem m_MuzzleFlash;
	private ParticleSystem m_Casings;
	private ParticleSystem m_Magazine;
	float fireTimer = 0;
	private int m_Ammo = 30;
    private bool m_IsReloading;

	// Use this for initialization
	void Start ()
	{
		m_Barrel = transform.Find("Barrel");
		m_MuzzleFlash = m_Barrel.Find("MuzzleFlash").GetComponent<ParticleSystem>();
		m_Casings = m_Barrel.Find("Casings").GetComponent<ParticleSystem>();
		m_Magazine = m_Barrel.Find("Magazine").GetComponent<ParticleSystem>();
	}

	void Fire()
	{
		if (m_Ammo > 0 && !m_IsReloading && Object.FindObjectOfType<Player>().IsAlive())
		{
			var go = PoolManager.manager.GetPooledObject("Bullet");
			go.transform.position = m_Barrel.position;
			go.transform.rotation = Quaternion.Lerp(m_Barrel.rotation, Random.rotation, 0.025f);
			fireTimer += fireInterval;
			Object.FindObjectOfType<CameraManager>().Kick();
			m_MuzzleFlash.Emit(5);
			m_Casings.Emit(1);
			AudioSource.PlayClipAtPoint(gunshotSound, transform.position);
			m_Ammo--;
			Object.FindObjectOfType<MagazineUI>().SetAmmo(m_Ammo);
		}
		else if (!m_IsReloading && Object.FindObjectOfType<Player>().IsAlive())
		{
			fireTimer += fireInterval;
			AudioSource.PlayClipAtPoint(emptySound, transform.position);

			StartCoroutine(Reload());
		}
	}


    private IEnumerator Reload()
    {
        m_IsReloading = true;
        // play sound

		transform.Find("Sprite").GetComponent<Animator>().SetTrigger("Reload");
		AudioSource.PlayClipAtPoint(reloadSound, transform.position);

		m_Magazine.Emit(1);

        yield return new WaitForSeconds(1.0f);
        m_Ammo = 30;
        m_IsReloading = false;
		Object.FindObjectOfType<MagazineUI>().SetAmmo(m_Ammo);
    }

    // Update is called once per frame
    void Update ()
	{
		fireTimer = Mathf.Max(fireTimer - Time.deltaTime, 0);

        if ( Input.GetKeyDown(KeyCode.R) && m_Ammo < 30 && !m_IsReloading && Object.FindObjectOfType<Player>().IsAlive())
        {
            StartCoroutine(Reload());
        }

		// Orient gun rotation
		Vector2 coord = Camera.main.WorldToScreenPoint(transform.position);

		var v1 = new Vector2(1, 0);
		var v2 = coord - new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		var rot = transform.localRotation.eulerAngles;
		if (coord.y > Input.mousePosition.y)
			rot.y = -Vector2.Angle(v1, v2) - 180;
		else
			rot.y = Vector2.Angle(v1, v2) + 180;
		transform.localRotation = Quaternion.Euler(rot);

		if (automatic)
		{
			if (Input.GetMouseButton(0) && fireTimer < Mathf.Epsilon)
				Fire();
		}
		else
		{
			if (Input.GetMouseButtonDown(0) && fireTimer < Mathf.Epsilon)
				Fire();
		}
	}
}
