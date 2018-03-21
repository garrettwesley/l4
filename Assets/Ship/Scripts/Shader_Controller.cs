using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_Controller : MonoBehaviour {

/*	private float velocity = 1f;

	float num = 0;
	float max = 1000;
	float increment = 1f;
	string label;
	bool increase = false;

	void Update()
	{
		label = num.ToString ();
		if(increase && num < max)
			num += increment;
		if(Input.GetKeyDown (KeyCode.E))
			increase = !increase;*/





	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Shader Forge/Dissolve1");
	}
	void Update() {

		//float slider = Mathf.SmoothDamp (1.0f, 0f, ref velocity, 0.1f);
		float slider = Mathf.PingPong(Time.time, 1.0F);
		rend.material.SetFloat("_dissolvo", slider);
	}
}