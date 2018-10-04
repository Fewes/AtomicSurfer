using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PlayerView : MonoBehaviour
{
	[SerializeField] [Range(0f, 1f)] float intensity;
	private float intensityTarget;
	public float brightness;
	public float contrast;
	private Material material;
	public Texture fx;

	public void SetIntensity(float f)
	{
		if (Mathf.Approximately(f, 1f))
			intensity = f;
		intensityTarget = f;
	}
	public float GetIntensity()
	{
		return intensity;
	}

	// Creates a private material used to the effect
	void Awake ()
	{
		material = new Material( Shader.Find("Hidden/PlayerView") );
	}
	
	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		/*
		if (intensity == 0)
		{
			Graphics.Blit (source, destination);
			return;
		}
		*/

		material.SetTexture("_FX", fx);
		material.SetFloat("_Blend", intensity);
		material.SetFloat("_Brightness", brightness);
		material.SetFloat("_Contrast", contrast);
		Graphics.Blit (source, destination, material);
	}

	void Update()
	{
		//intensity = Mathf.Lerp(intensity, intensityTarget, Time.deltaTime / 0.1f);
	}
}