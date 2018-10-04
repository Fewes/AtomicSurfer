using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
	public Gradient gradient;
	public string str;
	public bool temporary = true;

	private Transform t1;
	private Transform t2;
	private Transform t3;
	private Transform t4;
	private Transform t5;

	private Material material1;
	private Material material2;
	private Material material3;
	private Material material4;
	private Material material5;

	private TextMesh text1;
	private TextMesh text2;
	private TextMesh text3;
	private TextMesh text4;
	private TextMesh text5;

	public float offset;

	private float t;
	private float life = 2f;

	// Use this for initialization
	void Start ()
	{
		t1 = transform.Find("Text1");
		t2 = transform.Find("Text2");
		t3 = transform.Find("Text3");
		t4 = transform.Find("Text4");
		t5 = transform.Find("Text5");

		material1 = t1.GetComponent<MeshRenderer>().material;
		material2 = t2.GetComponent<MeshRenderer>().material;
		material3 = t3.GetComponent<MeshRenderer>().material;
		material4 = t4.GetComponent<MeshRenderer>().material;
		material5 = t5.GetComponent<MeshRenderer>().material;

		text1 = t1.GetComponent<TextMesh>();
		text2 = t2.GetComponent<TextMesh>();
		text3 = t3.GetComponent<TextMesh>();
		text4 = t4.GetComponent<TextMesh>();
		text5 = t5.GetComponent<TextMesh>();

		t2.localPosition = new Vector3(1, -1, 1) * offset * 1;
		t3.localPosition = new Vector3(1, -1, 1) * offset * 2;
		t4.localPosition = new Vector3(1, -1, 1) * offset * 3;
		t5.localPosition = new Vector3(1, -1, 1) * offset * 4;

		text1.text = str;
		text2.text = str;
		text3.text = str;
		text4.text = str;
		text5.text = str;
	}

	void OnEnable ()
	{
		life = 2f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		life -= Time.deltaTime;

		if (life < 0 && temporary)
			gameObject.SetActive(false);

		t += Time.deltaTime;
		if (t > 1) t -= 1;

		float t1 = t + 0.2f;
		float t2 = t + 0.4f;
		float t3 = t + 0.6f;
		float t4 = t + 0.8f;

		material1.SetColor("_Color", gradient.Evaluate(Time.time % 1));
		material2.SetColor("_Color", gradient.Evaluate((Time.time + 0.2f) % 1));
		material3.SetColor("_Color", gradient.Evaluate((Time.time + 0.4f) % 1));
		material4.SetColor("_Color", gradient.Evaluate((Time.time + 0.6f) % 1));
		material5.SetColor("_Color", gradient.Evaluate((Time.time + 0.8f) % 1));

		if (temporary)
			transform.position += Vector3.up * Time.deltaTime;
	}
}
