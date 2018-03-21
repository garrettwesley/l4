using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;

public class LaserControlPanelMaster : MonoBehaviour
{
    //Script Written by R.J. Susi for Lux Science game StarQuakeAcadamy
    //May contain pre-written code written by Roman Kazantsev, Ashish Basuray or Rahul Singh

    private Killvolume kv;
    private reloadSpaceship rs;
    private PauseMenu pauseMenu;
    private bool CursorIsVisible;

    public Camera FPCCam;
    public Camera ControlPanelCam;
    public FirstPersonController FPC;
    public GameObject OrbHUD;
    public DragNDropMiniGame DnDMini;
    public Canvas Sim;
    public Canvas DnD_Incomplete;
    public GameObject Laser_Sim_Panel;


    // Use this for initialization
    void Start()
    {
        Sim.enabled = false;
        DnD_Incomplete.enabled = false;
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        kv = GameObject.Find("KillVolume").GetComponent<Killvolume>();
        rs = GameObject.Find("LevelLoader").GetComponent<reloadSpaceship>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CursorIsVisible == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ControlPanelCam.enabled = true;
        FPCCam.enabled = false;
        FPC.GetComponent<FirstPersonController>().enabled = false;
        OrbHUD.SetActive(false);
        pauseMenu.pauseMenuAccessible = false;

        CursorIsVisible = true;

        if(DnDMini.getGameComplete())
        {
            DnD_Incomplete.enabled = false;
            Sim.enabled = true;
            Laser_Sim_Panel.SetActive(true);
        }
        else
        {
            DnD_Incomplete.enabled = true;
            Sim.enabled = false;
        }
    }

    public void ReturnToFPCviaExit()
    {
        ControlPanelCam.enabled = false;
        FPCCam.enabled = true;
        FPC.GetComponent<FirstPersonController>().enabled = true;
        OrbHUD.SetActive(true);
        pauseMenu.pauseMenuAccessible = true;

        CursorIsVisible = false;
        Laser_Sim_Panel.SetActive(false);
        Sim.enabled = false;
        DnD_Incomplete.enabled = false;
    }
}
