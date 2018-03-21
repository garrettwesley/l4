using DigitalRuby.LightningBolt;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliumAtom : MonoBehaviour
{
    private bool translatingUp;
    private bool translatingDown;
    private GameObject lightningBolt;
	public int UIbuttonClickCounter;

    public bool ShowUI = true;
    public float TranslationRate = 1.0f;
    public float LightningShowTime = 1f;
    public Camera Camera;
    public GameObject Helium;
    public RectTransform UpArrow;
    public RectTransform DownArrow;
    public RectTransform UIButton;
    public GameObject Excitation;
    public GameObject LightningBolt;
    public GameObject Underneath;
    public GameObject UICanvas;
	public bool Excitable;
	public bool StartLoop;
    public bool InLesson = false;
	public Animator EnergyLevel;

	int eUp = Animator.StringToHash("Electron_ED_up");
    // ------------------------------------------------------------------------------------- //

    public void RefreshUI()
    {
        
		if (ShowUI == true)
		{
			this.UICanvas.SetActive(this.ShowUI);

		}
        if (!this.ShowUI)
        {
            return;
        }

        if (this.Camera == null)
        {
            return;
        }

        Vector2 upArrowPoint =
            this.Camera.WorldToScreenPoint(this.Helium.gameObject.transform.position + new Vector3(0, 4, 0));

        this.UpArrow.anchoredPosition = upArrowPoint;

        Vector2 downArrowPoint =
            this.Camera.WorldToScreenPoint(this.Helium.gameObject.transform.position - new Vector3(0, 6, 0));

        this.DownArrow.anchoredPosition = downArrowPoint;

        if (this.Underneath != null)
        {
            Vector2 excitePoint =
                this.Camera.WorldToScreenPoint(this.Underneath.transform.position);
            this.UIButton.anchoredPosition = excitePoint;
        }
    }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator DoExcitation(TimeSpan lightningTime)
    {
        if (Excitation == null) { yield return new WaitForSeconds((float)lightningTime.TotalSeconds); }
        else
        {
            this.Excitation.SetActive(true);
            this.Excitation.GetComponent<Excitation>().BumpElectronToOuterShell();

			if (this.LightningBolt == null)
			{
				yield break;
			}

			else
			{
				this.LightningBolt.SetActive(true);
				yield return new WaitForSeconds((float)lightningTime.TotalSeconds);
				this.LightningBolt.SetActive(false);	
			}

        }
    }

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
        RefreshUI();

        var circlifies = this.GetComponentsInChildren<Circlify>();
        float minRadius = float.MaxValue;
        float maxRadius = float.MinValue;
        foreach (var circlify in circlifies)
        {
            if (circlify.Radius < minRadius)
            {
                minRadius = circlify.Radius;
            }
            if (circlify.Radius > maxRadius)
            {
                maxRadius = circlify.Radius;
            }
        }
        this.Excitation.GetComponent<Excitation>().SetRadii(minRadius, maxRadius);
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
//		if (Input.GetKeyDown(KeyCode.Space) && Excitable == true )
//		{
//			StartLoop = true;
//		}
//		
//		if (StartLoop == true)
//		{
//            Excite();
//			StartLoop = false;
//        }

	

        RefreshUI();
        if (!this.translatingUp && !this.translatingDown)
        {
            return;
        }

        if (this.translatingUp)
        {
            this.gameObject.transform.position +=
                new Vector3(0, Time.deltaTime * this.TranslationRate, 0);

            if (this.UpArrow.anchoredPosition.y - this.UpArrow.sizeDelta.y / 2 > Input.mousePosition.y)
            {
                this.translatingUp = false;
            }
        }
        else if (this.translatingDown)
        {
            this.gameObject.transform.position -=
                new Vector3(0, Time.deltaTime * this.TranslationRate, 0);

            if (this.DownArrow.anchoredPosition.y + this.UpArrow.sizeDelta.y / 2 < Input.mousePosition.y)
            {
                this.translatingDown = false;
            }
        }

        RefreshUI();
        var splines = this.gameObject.GetComponentsInChildren<CatmullRomSpline>();
        foreach (var spline in splines)
        {
            spline.RefreshLinePoints();
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void BeginTranslateUp()
    {
        this.translatingUp = true;
    }

    // ------------------------------------------------------------------------------------- //

    public void BeginTranslateDown()
    {
        this.translatingDown = true;
    }

    // ------------------------------------------------------------------------------------- //

    public void StopTranslation()
    {
        this.translatingUp = false;
        this.translatingDown = false;
    }

    // ------------------------------------------------------------------------------------- //

    public void Excite()
    {
        StartCoroutine(DoExcitation(TimeSpan.FromSeconds(this.LightningShowTime)));
		if (EnergyLevel == null)
		{
			return;
		}
		this.EnergyLevel.SetTrigger(eUp);

	}

    // ------------------------------------------------------------------------------------- //

	public void ButtonClickCounter()
	{
		UIbuttonClickCounter++;

	}
}
