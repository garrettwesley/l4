using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FiredPhoton : MonoBehaviour
{
    public CapsuleCollider ParentCollider;
	public CapsuleCollider ElectronCollector;

    public bool Loop;
	public bool LaserMiniGame;
    public float DistanceToDestroy;
    public float RotSpeed;
    public float delayInSeconds;
    public Vector3 Velocity;

    public HeliumAtom ParentHelium
    {
        get;
        set;
    }

    // ------------------------------------------------------------------------------------- //

    public void Start()
    { }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
        this.gameObject.transform.position += this.Velocity * Time.deltaTime;
        this.transform.localRotation = Quaternion.Euler(this.RotSpeed * Time.deltaTime, 0, 0);

        if ((this.gameObject.transform.position - this.ParentHelium.transform.position).magnitude > this.DistanceToDestroy)
        {
            print("destroying fired photon...");
			if (this.Loop == true)
            {
                StartCoroutine(ExciteParentAfterDelay());
                foreach (var renderer in base.GetComponents<Renderer>())
                {
                    renderer.enabled = false;
                }
                foreach (var renderer in base.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
                this.Loop = false;
            }
        }
    }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator ExciteParentAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        this.ParentHelium.Excite();
        Destroy(this.gameObject);
    }

    // ------------------------------------------------------------------------------------- //

    public void OnTriggerEnter(Collider collider)
    {
        if (collider == this.ParentCollider)
        {
            return;
        }

		if (LaserMiniGame == true)
		{
			GameObject laserMiniGame = GameObject.Find("LaserMiniGame_Controller");
			laserMiniGame.GetComponent<LaserMiniGame>().HandleElectronCollision(this, collider);
			GameObject.Destroy(this.gameObject);
		}

    }

    // ------------------------------------------------------------------------------------- //
}
