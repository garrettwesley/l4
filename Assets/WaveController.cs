using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WaveController : MonoBehaviour {

	private GameObject[] cubes;
	private Vector3[] pos;
	private Vector3[] scale;
	private float slidervalue_wave;
	private float slidervalue_amp;
	private int currenttexture;


	public GameObject quad;
	public GameObject emptyGO;
	public Transform origin;
	public int NumOfCubes;
	public Texture[] textures;
	public Renderer rend;




	// ------------------------------------------------------------------------------------- //

	void Start () {

		Renderer rend = quad.GetComponent<Renderer> ();
		rend.material.shader = Shader.Find ("Shader Forge/WaveControllerShader");

		float temp = newValue (1f, 0.25f, 1.2f, 0f, 1f);
		rend.material.SetColor ("_Color", new Color(1f - temp, 0f, 1f));


		pos = new Vector3[NumOfCubes];
		cubes = new GameObject[NumOfCubes];
		scale = new Vector3[NumOfCubes];

		for (int i=0; i< NumOfCubes; i++)
		{
			this.cubes[i] = GameObject.Instantiate (this.quad, origin.position + new Vector3(i,0,0) * this.quad.transform.localScale.x , this.origin.rotation);
			this.cubes [i].transform.parent = this.emptyGO.transform;

		}

	}
	// ------------------------------------------------------------------------------------- //

	void Update () 
	{



		if (slidervalue_wave >= 4.05f && slidervalue_wave <= 5f)
		{

			float temp = newValue (slidervalue_wave, 4.05f, 5f, 0f, 1f);
			rend.material.SetColor ("_Color", new Color(1f, 1f-temp, 0f));
		}

		if (slidervalue_wave >= 3.1f && slidervalue_wave <= 4.05f)
		{

			float temp = newValue (slidervalue_wave, 3.1f, 4.05f, 0f, 1f);
			rend.material.SetColor ("_Color", new Color(temp, 1f, 0f));
		}

		if (slidervalue_wave >= 2.15f && slidervalue_wave <= 3.1f)
		{

			float temp = newValue (slidervalue_wave, 2.15f, 3.1f, 0f, 1f);
			rend.material.SetColor ("_Color", new Color(0f, 1f, 1f - temp));
		}

		if (slidervalue_wave >= 1.2f && slidervalue_wave <= 2.15f)
		{

			float temp = newValue (slidervalue_wave, 1.2f, 2.15f, 0f, 1f);
			rend.material.SetColor ("_Color", new Color(0f, temp, 1f));
		}

		if (slidervalue_wave >= 0.25f && slidervalue_wave <= 1.2f)
		{

			float temp = newValue (slidervalue_wave, 0.25f, 1.2f, 0f, 1f);
			rend.material.SetColor ("_Color", new Color(1f - temp, 0f, 1f));
		}



	}

	public float newValue (float value, float low1, float high1, float low2, float high2)
	{
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);


	}




	// ------------------------------------------------------------------------------------- //

	public void Wavelength_slider(float slidervalue)
	{
		slidervalue_wave = slidervalue;
//		this.quad.GetComponent<Renderer> ().enabled = false;
		for (int i=0; i< NumOfCubes; i++)
		{
			pos[i] = cubes [i].transform.position;
			pos [i].x = i * slidervalue * this.quad.transform.localScale.x + this.emptyGO.transform.position.x;
			cubes [i].transform.position = pos[i];

			scale [i] = cubes [i].transform.localScale;
			scale [i].x = slidervalue * this.quad.transform.localScale.x;
			cubes [i].transform.localScale = scale [i];

		}
	}
	// ------------------------------------------------------------------------------------- //

		public void Amplitude_slider(float slidervalue)
		{
		slidervalue_amp = slidervalue;
			for (int i=0; i< NumOfCubes; i++)
			{
				scale [i] = cubes [i].transform.localScale;
				scale [i].y = slidervalue;
				cubes [i].transform.localScale = scale [i];
			}
		}
	// ------------------------------------------------------------------------------------- //
		
	public void FlipPolarity()
	{
		currenttexture++;
		currenttexture %= textures.Length;

		for (int i=0; i< NumOfCubes; i++)
		{
//			this.cubes [i].GetComponent<Renderer> ().material.mainTexture = textures [currenttexture];
			rend.material.SetTexture ("_node_3856", textures [currenttexture]);
		}
		
	}



}
	
