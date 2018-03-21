using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mirror_Controller : MonoBehaviour {

    private int Num_Of_Collisions;
    private int Num_Of_Collisions_Allowed;
    private float Mirror_Reflectivity;
    private Collider My_Collider;
    private Collider Physical_Collider;

    public Slider Mirror_Reflectivity_Slider;
    public laser laser_Script;
    public GameObject My_Child;
    

    // Use this for initialization
    void Start ()
    {
        My_Collider = GetComponent<Collider>();
        Mirror_Reflectivity = Mirror_Reflectivity_Slider.value;
        Num_Of_Collisions_Allowed = (int)(Mirror_Reflectivity * 100);

        Physical_Collider = My_Child.GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Mirror_Reflectivity_Slider.value == 1)
        {
            Num_Of_Collisions_Allowed = 10000;
        }
        else
        {
            Num_Of_Collisions_Allowed = (int)(Mirror_Reflectivity * 10);
        }
        Mirror_Reflectivity = Mirror_Reflectivity_Slider.value;
	}

    private void OnTriggerEnter(Collider collider)
    {   
        //if the collision was done by a photon
        for(int i = 0; i < laser_Script.getCurrentNumOfPhotons(); i++)
        {
            if(laser_Script.getPhoton(i) == null)
            {
                continue;
            }
            if(collider.attachedRigidbody == laser_Script.getPhoton(i).GetComponent<Rigidbody>())
            {
                Num_Of_Collisions++;
                if(Num_Of_Collisions_Allowed == 0)
                {
                    Physics.IgnoreCollision(Physical_Collider, collider);
                }
                else if (Num_Of_Collisions >= Num_Of_Collisions_Allowed && Num_Of_Collisions_Allowed != 10000)
                {
                    Physics.IgnoreCollision(Physical_Collider, collider);
                    Num_Of_Collisions = 0;
                }
            }
        }
    }
}
