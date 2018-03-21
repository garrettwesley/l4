using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Excitation : MonoBehaviour
{
    static class Constants
    {
        public const float DualElectronVerticalOffset = 0.75f;
    }

    private bool decreasing;
    private float innerShellRadius;
    private float outerShellRadius;
    private Interpolator excitationInterpolator;
    private Interpolator spline2Interpolator;
    private DateTime waitingAtOuterShellTime;

    public bool LoopPhoton;
    public float StartScale = 0.6f;
    public float Duration = 1.25f;
    public float SecondsToWaitAtOuterShell = 0.5f;
    public Animator EnergyLevel;
    public GameObject EnergyDiagram;
    public bool EmissionDemo = false;
    public Spontaneous_emission sponEm;
    public float sponEmDelayTime = 1f;
	public bool ExcitationOnly_noPhoton;

    

    public GameObject Photon;

    public bool SpawnSecondPhoton
    {
        get;
        set;
    }

    public float CurrentPercentage
    {
        get
        {
            if (this.excitationInterpolator == null)
            {
                return 0;
            }
            return (this.excitationInterpolator.CurrentValue - this.StartScale) /
                (this.outerShellRadius - this.StartScale);
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void Awake()
    {
        this.excitationInterpolator = new Interpolator();
        this.excitationInterpolator.SetParameters(this.StartScale, this.outerShellRadius * 2, this.Duration);



        this.spline2Interpolator = new Interpolator();
        this.spline2Interpolator.SetParameters(this.innerShellRadius, this.outerShellRadius, this.Duration);
    }

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {

        StartCoroutine(HeExciteAnim());
    }


    // ------------------------------------------------------------------------------------- //


    private void SetSplineMoving(bool moving)
    {
        var splines = base.gameObject.transform.parent.GetComponentsInChildren<CatmullRomSpline>();
        foreach (CatmullRomSpline spline in splines)
        {
            spline.IsMoving = moving;
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void SetRadii(float inner, float outer)
    {
        this.innerShellRadius = inner;
        this.outerShellRadius = outer;
    }

    // ------------------------------------------------------------------------------------- //

    private void ChangeSpline2Radius(float radius)
    {
        var circlifies = base.transform.parent.GetComponentsInChildren<Circlify>();
        foreach (Circlify circlify in circlifies)
        {
            if (circlify.Parent.name == "Spline2ControlPoints")
            {
                circlify.Radius = radius;
                circlify.DoCirclify();
                break;
            }
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void BumpElectronToOuterShell()
    {
        this.excitationInterpolator.Restart();
        this.spline2Interpolator.Restart();
        this.decreasing = false;
        this.SpawnSecondPhoton = false;
        SetSplineMoving(false);

        

    }

    // ------------------------------------------------------------------------------------- //

    private GameObject InstantiatePhoton()
    {
        GameObject go = GameObject.Instantiate(this.Photon);
        var photon = go.GetComponent<FiredPhoton>();
        //        photon.Loop = this.LoopPhoton;
        photon.ParentHelium = this.gameObject.transform.parent.GetComponent<HeliumAtom>();
        return go;
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
        if (DateTime.Now < this.waitingAtOuterShellTime)
        {
            return;
        }

        if (EmissionDemo == true)
        {

            //Debug.Log ("part1");


            this.excitationInterpolator.Update(Time.deltaTime * (this.decreasing ? -1 : 1));
            this.spline2Interpolator.Update(Time.deltaTime * (this.decreasing ? -1 : 1));
            ChangeSpline2Radius(this.spline2Interpolator.CurrentValue);
            this.gameObject.transform.localScale =
                new Vector3(
                this.excitationInterpolator.CurrentValue,
                this.gameObject.transform.localScale.y,
                this.excitationInterpolator.CurrentValue);
            if (!this.decreasing)
            {
                if (this.excitationInterpolator.CurrentValue >= this.outerShellRadius * 2)
                {


                    this.decreasing = true;
                    StartCoroutine(delayedPhotonSpawn());
                    this.waitingAtOuterShellTime = DateTime.Now + TimeSpan.FromSeconds(this.SecondsToWaitAtOuterShell);
                }
            }
            else
            {
                if (this.excitationInterpolator.CurrentValue <= this.StartScale)
                {

                    ChangeSpline2Radius(this.innerShellRadius);
                    SetSplineMoving(true);
                    base.gameObject.SetActive(false);

                    this.sponEm.SpawnPhoton_double();

                }

            }
        }

        else

        {
            //Debug.Log ("part2");
            this.excitationInterpolator.Update(Time.deltaTime * (this.decreasing ? -1 : 1));
            this.spline2Interpolator.Update(Time.deltaTime * (this.decreasing ? -1 : 1));
            ChangeSpline2Radius(this.spline2Interpolator.CurrentValue);
            this.gameObject.transform.localScale =
                new Vector3(
                    this.excitationInterpolator.CurrentValue,
                    this.gameObject.transform.localScale.y,
                    this.excitationInterpolator.CurrentValue);
            if (!this.decreasing)
            {
                if (this.excitationInterpolator.CurrentValue >= this.outerShellRadius * 2)
                {
                    if (this.EnergyLevel != null)
                    {
                         this.EnergyLevel.SetTrigger("Electron_ED_up");
                    }

                    this.decreasing = true;
                    this.waitingAtOuterShellTime = DateTime.Now + TimeSpan.FromSeconds(this.SecondsToWaitAtOuterShell);
                }
            }
            else
            {
                if (this.excitationInterpolator.CurrentValue <= this.StartScale)
                {
//                    if (this.EnergyLevel != null)
//                    {
//                        this.EnergyLevel.SetTrigger("Electron_ED_down");
//
//                    }
                    ChangeSpline2Radius(this.innerShellRadius);
                    SetSplineMoving(true);
                    base.gameObject.SetActive(false);
					if (ExcitationOnly_noPhoton == true)
					{
						return;
					}
					else{
						SpawnPhoton();
					}
                    
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator delayedPhotonSpawn()
    {
        yield return new WaitForSeconds(sponEmDelayTime);

        if (sponEm != null)
        {
            this.sponEm.SpawnPhoton_sponEm();
        }

    }

    private void SpawnPhoton()
    {
        //Debug.Log ("part3");

        GameObject go = InstantiatePhoton();
        go.SetActive(true);
        //Debug.Log ("part4");

        go.transform.position =
            this.transform.position - new Vector3(0, this.SpawnSecondPhoton ? -Constants.DualElectronVerticalOffset : 0, 0);

        if (this.SpawnSecondPhoton)
        {
            GameObject go2 = InstantiatePhoton();
            go2.transform.position =
                this.transform.position - new Vector3(0, this.SpawnSecondPhoton ? Constants.DualElectronVerticalOffset : 0, 0);
            go2.SetActive(true);
        }
    }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator HeExciteAnim()
    {
        yield return new WaitForSeconds(0);

        int HeTrigger = Animator.StringToHash("He_Excite");
        int HeTrigger2 = Animator.StringToHash("Start_HeNe");
		if (EnergyLevel != null)
		{
			this.EnergyLevel.SetBool(HeTrigger, true); //Animate Excitation Clyinder (amendment to EnergyLevel)
			this.EnergyLevel.SetBool(HeTrigger2, true); //Animate Excitation Clyinder (amendment to EnergyLevel)
		}
        
        Debug.Log("HeExciteCylinder Excited");
    }
}
