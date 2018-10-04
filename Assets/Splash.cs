using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}

	void SplashFinished ()
	{
		gameObject.SetActive(false);
	}

	void OnEnable ()
	{
		GetComponent<Animator>().SetTrigger("Splash");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
