using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour {

    private Killvolume kv;
    private reloadSpaceship rs;
    private PauseMenu pauseMenu;

    public GameObject doorWaySphere;
    public GameObject doorWayPlane;
    public Material laserBeamRed;

    // Use this for initialization
    void Start ()
    {
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        kv = GameObject.Find("KillVolume").GetComponent<Killvolume>();
        rs = GameObject.Find("LevelLoader").GetComponent<reloadSpaceship>();
    }

    public void ActivateDoorWay()
    {
        //FullLaser_inHolodeck.SetActive (true);
        doorWaySphere.GetComponent<Renderer>().material = laserBeamRed;
        doorWayPlane.SetActive(true);
        rs.HolodeckComplete = true;
    }
}
