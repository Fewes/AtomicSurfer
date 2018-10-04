using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flasher : MonoBehaviour
{
	public float flashFade = 0.25f;

	private Material material;
	private float flashTimer;

	// Use this for initialization
	void Start ()
	{
		var renderer = GetComponent<MeshRenderer>();
		if (renderer)
		{
			material = renderer.material;
		}
		else
		{
			var sRenderer = GetComponent<SpriteRenderer>();
			if (sRenderer)
			{
				material = sRenderer.material;
			}
		}
	}

	public void Flash()
	{
		flashTimer = 1;
	}

	public void ResetFlash()
	{
		if (material)
			material.SetFloat("_FlashAmount", 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		flashTimer -= Time.deltaTime / flashFade;

		if (flashTimer > Mathf.Epsilon)
			material.SetFloat("_FlashAmount", flashTimer);
		else
			material.SetFloat("_FlashAmount", 0);
	}
}
