using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron_Controller : MonoBehaviour {

    private Rigidbody My_RigidBody;
    private float x;
    private float y;

	// Use this for initialization
	void Start ()
    {
        My_RigidBody = GetComponent<Rigidbody>();

        x = 0;
        y = 0;
        
        while(x == 0)
        {
            x = Random.Range(-2, 2);
        }

        while(y == 0)
        {
            y = Random.Range(-2, 2);
        }

        My_RigidBody.AddForce(new Vector3(x, y, 0));
    }

	void FixedUpdate ()
    {
        My_RigidBody.velocity = 2 * (My_RigidBody.velocity.normalized);
    }
}
