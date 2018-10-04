using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
	public GameObject bulletUI;

	private int capacity = 30;
	private List<GameObject> bullets;

	// Use this for initialization
	void Start ()
	{
		bullets = new List<GameObject>();

		for (int i = 0; i < capacity; i++)
		{
			var go = Instantiate(bulletUI);
			go.transform.parent = transform.parent;
			var pos = go.GetComponent<RectTransform>().position;
			pos.y += 10 * i;
			go.GetComponent<RectTransform>().position = pos;
			bullets.Add(go);
		}

	}

	public void SetAmmo(int ammo)
	{
		for (int i = 0; i < bullets.Count; i++)
		{
			if (i >= ammo)
				bullets[i].GetComponent<CanvasRenderer>().cull = true;
			else
				bullets[i].GetComponent<CanvasRenderer>().cull = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
