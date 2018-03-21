using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Coherence : MonoBehaviour
{
    private GameObject selectedSineWave;
    private List<GameObject> snappedSineWaves;

    public float UVOffsetPerSec = .15f;
    public float SnapTolerance = 0.1f;
    public GameObject[] SineWaves;
    public Texture SnappedTexture;
    public Camera HUDCamera;

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
        this.snappedSineWaves = new List<GameObject>();

        for (int i = 0; i < this.SineWaves.Length; i++)
        {
            var m = this.SineWaves[i].GetComponent<Renderer>().material;
            m.mainTextureOffset = new Vector2(1.0f / this.SineWaves.Length * i, m.mainTextureOffset.y);
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
        foreach (GameObject go in this.SineWaves)
        {
            if (go == this.selectedSineWave)
            {
                continue;
            }
            var m = go.GetComponent<Renderer>().material;
            float x = (m.mainTextureOffset.x + Time.deltaTime * this.UVOffsetPerSec);
            if (x > 1)
            {
                x -= 1;
            }
            m.mainTextureOffset = new Vector2(x, m.mainTextureOffset.y);
        }

        if (!Input.GetMouseButton(0))
        {
            if (this.selectedSineWave != null)
            {
                float x1 = this.selectedSineWave.GetComponent<Renderer>().material.mainTextureOffset.x;
                IEnumerable<GameObject> list = this.snappedSineWaves.Any() ? this.snappedSineWaves as IEnumerable<GameObject> : this.SineWaves;
                foreach (GameObject sineWave in list)
                {
                    if (sineWave == this.selectedSineWave)
                    {
                        continue;
                    }

                    float x2 = sineWave.GetComponent<Renderer>().material.mainTextureOffset.x;
                    float delta = Mathf.Abs(x2 - x1);
                    if (delta < this.SnapTolerance)
                    {
                        if (!snappedSineWaves.Contains(sineWave))
                        {
                            this.snappedSineWaves.Add(sineWave);
                            sineWave.GetComponent<Renderer>().material.mainTexture = this.SnappedTexture;
                        }
                        this.snappedSineWaves.Add(this.selectedSineWave);
                        this.selectedSineWave.GetComponent<Renderer>().material.mainTexture = this.SnappedTexture;
                        break;
                    }
                }
            }
            this.selectedSineWave = null;
            return;
        }
        //create camera variable, link to lesson's camera
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ray = HUDCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!this.snappedSineWaves.Contains(hit.collider.transform.gameObject))
            {
                this.selectedSineWave = hit.collider.transform.gameObject;
            }
        }
    }

    // ------------------------------------------------------------------------------------- //
}
