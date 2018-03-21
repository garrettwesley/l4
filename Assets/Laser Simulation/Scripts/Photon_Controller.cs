using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Photon_Controller : MonoBehaviour {

    private Rigidbody My_RigidBody;
    public Slider Mirror_Reflection_Slider;

	// Use this for initialization
	void Start ()
    {
        My_RigidBody = GetComponent<Rigidbody>();
        if (Mirror_Reflection_Slider.value > 0.15)
        {
            My_RigidBody.AddForce(new Vector3(4, 0, 0));
        }
        else
        {
            float x = 0;
            float y = 0;
            while(x == 0 && y == 0)
            {
                x = Random.Range(-4, 4);
                y = Random.Range(-4, 4);
            }
            My_RigidBody.AddForce(new Vector3(x, y, 0));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        My_RigidBody.velocity = 4 * (My_RigidBody.velocity.normalized);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (Mirror_Reflection_Slider.value < 0.15)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
}
