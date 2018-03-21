using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoyer_Of_Photons : MonoBehaviour {

    private int Num_Of_Photons_Destroyed;
    private int Num_Of_Photons_Destoryed_Per_Second;

    public laser laser_Script;

    public void Start()
    {
        Num_Of_Photons_Destroyed = 0;
        InvokeRepeating("resetDestructionsPerSecond", 0f, 0.25f);//Repeates resetDestructionsPerSecond once every second to reset the number
    }

    public void OnCollisionEnter(Collision collision)
    {
        int PhotonI = laser_Script.getPhotonI(collision.gameObject);
        laser_Script.addToPhotonList(PhotonI);
        Destroy(collision.gameObject, 0);
        Num_Of_Photons_Destroyed++;
    }

    public void resetDestructionsPerSecond()//Resets the counter for number of photons destoryed back to 0
    {
        Num_Of_Photons_Destoryed_Per_Second = Num_Of_Photons_Destroyed;
        Num_Of_Photons_Destroyed = 0;
    }

    public int getPhotonsDestroyedPerSecond()
    {
        return Num_Of_Photons_Destoryed_Per_Second;
    }
}
