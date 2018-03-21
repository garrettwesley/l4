using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LaserCavityMiniGame : MonoBehaviour
{
    private float currentRotation;
    private Vector3 lastHitPoint;
    private LineRenderer leftLine;
    private LineRenderer middleLine;
    private LineRenderer rightLine;
    private Material unlitTextureMaterial;

    public float InitialMirrorAngle = 120;
    public float RotationRate = 30;

    public GameObject LeftMirror;
    public GameObject RightMirror;
    public GameObject BackgroundPlane;
    public GameObject MagnifyGlassTexturePlane;
    public ParticleSystem ParticleSystem;
    public RectTransform MagnifyingGlass;
    public Camera MainCamera;
    public Camera MagnifyCamera;

    // ------------------------------------------------------------------------------------- //

    private LineRenderer CreateLineRenderer(string name, Color c, float width)
    {
        GameObject go = new GameObject(name);
        LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = this.unlitTextureMaterial;
        lineRenderer.material.color = c;
        go.transform.parent = this.transform;
        return lineRenderer;
    }

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
        Cursor.visible = false;

        this.unlitTextureMaterial = new Material(Shader.Find("Unlit/Color"));
        this.currentRotation = this.InitialMirrorAngle;
        this.leftLine = CreateLineRenderer("LeftLine", Color.red, 0.25f);
        this.middleLine = CreateLineRenderer("MiddleLine", Color.red, 0.25f);
        this.rightLine = CreateLineRenderer("RightLine", Color.red, 0.25f);

        this.middleLine.SetPositions(new Vector3[] {
            this.LeftMirror.gameObject.transform.position,
            this.RightMirror.gameObject.transform.position });

        Update();
    }

    // ------------------------------------------------------------------------------------- //

    private void UpdateMagnifier()
    {
        this.MagnifyingGlass.anchoredPosition =
            new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var ray = this.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.BackgroundPlane)
        {
            this.MagnifyGlassTexturePlane.transform.position = hit.point;
            Vector3 d = this.lastHitPoint - hit.point;
            this.MagnifyCamera.transform.position += d;
            this.lastHitPoint = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            this.MagnifyGlassTexturePlane.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.MagnifyGlassTexturePlane.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // ------------------------------------------------------------------------------------- //

    private void UpdateMirrorRotation()
    {
        bool e = Input.GetKey(KeyCode.E);
        bool r = Input.GetKey(KeyCode.R);
        if (!e && !r)
        {
            return;
        }
        if (e)
        {
            this.currentRotation += Time.deltaTime * this.RotationRate;
        }
        if (r)
        {
            this.currentRotation -= Time.deltaTime * this.RotationRate;
        }

        this.currentRotation = Mathf.Clamp(this.currentRotation, 0, 180);
        this.LeftMirror.gameObject.transform.localEulerAngles = new Vector3(0, 0, this.currentRotation);
        this.RightMirror.gameObject.transform.localEulerAngles = new Vector3(0, 0, this.currentRotation);
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
        UpdateMagnifier();
        UpdateMirrorRotation();
        UpdateLeftAndRightLines();
        UpdateMiddleLine();
        UpdateParticleSystem();
    }

    // ------------------------------------------------------------------------------------- //

    private void UpdateMiddleLine()
    {
        float angle = Math.Abs(this.currentRotation - 90);
        var lineRenderer = this.middleLine.GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = angle.Remap(0, 90, 3f, 1f);
    }

    // ------------------------------------------------------------------------------------- //

    private void UpdateParticleSystem()
    {
        float angle = Math.Abs(this.currentRotation - 90);
        var shape = this.ParticleSystem.shape;
        shape.radius = angle.Remap(0, 90, 0.5f, 5.5f);
    }

    // ------------------------------------------------------------------------------------- //

    private void UpdateLeftAndRightLines()
    {
        float lineDistance = (this.LeftMirror.gameObject.transform.position -
            this.RightMirror.gameObject.transform.position).magnitude;
        float cos = Mathf.Cos((this.currentRotation - 90) * Mathf.Deg2Rad);
        float sin = Mathf.Sin((this.currentRotation - 90) * Mathf.Deg2Rad);

        this.leftLine.SetPositions(new Vector3[] {
            this.LeftMirror.gameObject.transform.position,
            this.LeftMirror.gameObject.transform.position +
                lineDistance * new Vector3(cos, sin, 0) });

        cos = Mathf.Cos((this.currentRotation + 90) * Mathf.Deg2Rad);
        sin = Mathf.Sin((this.currentRotation + 90) * Mathf.Deg2Rad);

        this.rightLine.SetPositions(new Vector3[] {
            this.RightMirror.gameObject.transform.position,
            this.RightMirror.gameObject.transform.position +
                lineDistance * new Vector3(cos, sin, 0) });
    }

    // ------------------------------------------------------------------------------------- //

    public void OnDestroy()
    {
        Destroy(this.unlitTextureMaterial);
    }

    // ------------------------------------------------------------------------------------- //
}
