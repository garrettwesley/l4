using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NucleusRotation : MonoBehaviour
{
    public float RotationRate = 200;

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
        this.gameObject.transform.Rotate(
            Vector3.forward,
            -this.RotationRate * Time.deltaTime,
            Space.Self);

        this.gameObject.transform.Rotate(
            Vector3.up,
            this.RotationRate * Time.deltaTime,
            Space.World);
    }

    // ------------------------------------------------------------------------------------- //
}
