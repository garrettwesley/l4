using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circlify : MonoBehaviour
{
    public float Radius = 7;
    public Transform Parent;

    // ------------------------------------------------------------------------------------- //

    public void Awake()
    {
        DoCirclify();
    }

    // ------------------------------------------------------------------------------------- //

    public void DoCirclify()
    {
        if (this.Parent == null)
        {
            return;
        }

        for (int i = 0; i < this.Parent.childCount; i++)
        {
            float angle = i * Mathf.PI * 2 / this.Parent.childCount;
            float c = Mathf.Cos(angle) * this.Radius;
            float s = Mathf.Sin(angle) * this.Radius;
            this.Parent.GetChild(i).transform.position =
                this.Parent.transform.position +
                this.Parent.transform.right * c +
                this.Parent.transform.up * s;
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    { }

    // ------------------------------------------------------------------------------------- //
}
