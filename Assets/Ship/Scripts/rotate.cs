using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {


	public RectTransform rect;
	public float rotateSpeed;

	void Start () {



	}


	void Update () {
			

		rect.transform.Rotate(new Vector3(0,0,1), rotateSpeed * Time.deltaTime);

		/*rect.position = rect.transform.Rotate(new Vector3(0f,0f,1f) * Time.deltaTime * rotateSpeed);*/

	}
}
