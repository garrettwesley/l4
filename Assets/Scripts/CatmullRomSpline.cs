using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Taken from http://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/
//Interpolation between points with a Catmull-Rom spline
public class CatmullRomSpline : MonoBehaviour
{
    private int currentSegment;
    private float currentTime;
    private List<Vector3> linePoints = new List<Vector3>();

    /// <summary>
    /// Has to be at least 4 points
    /// </summary>
    public GameObject[] ControlPoints;

    public float TotalDurationOverride;
    public float[] SegmentDurations;

    public LineRenderer LineRenderer;

    public Transform ObjectToMoveAlongSpline;

    public bool IsMoving = true;

    public float SplineResolution = 0.2f;

    /// <summary>
    /// Are we making a line or a loop?
    /// </summary>
    public bool IsLooping = true;

    public float StartTime = 0;

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
        RefreshLinePoints();
        this.currentTime = this.StartTime;
		this.LineRenderer.positionCount = this.linePoints.Count;
        this.LineRenderer.SetPositions(this.linePoints.ToArray());
        if (this.TotalDurationOverride != 0)
        {
            int numSegments = this.ControlPoints.Length;
            if (!this.IsLooping)
            {
                numSegments = this.ControlPoints.Length - 2;
            }
            this.SegmentDurations = new float[numSegments];
            for (int i = 0; i < numSegments; i++)
            {
                this.SegmentDurations[i] = this.TotalDurationOverride / numSegments;
            }
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
        if (this.ObjectToMoveAlongSpline == null)
        {
            return;
        }
        if (this.currentSegment >= this.SegmentDurations.Length)
        {
            return;
        }
        if (this.IsMoving)
        {
            this.currentTime += Time.deltaTime;
        }
        float segmentEndTime = 0;
        for (int i = 0; i <= this.currentSegment; i++)
        {
            segmentEndTime += this.SegmentDurations[i];
        }
        if (this.currentTime > segmentEndTime)
        {
            this.currentSegment++;
            if (this.currentSegment == this.SegmentDurations.Length)
            {
                if (!this.IsLooping)
                {
                    return;
                }
                this.currentSegment = 0;
                this.currentTime -= segmentEndTime;
                segmentEndTime = 0;
            }
            segmentEndTime += this.SegmentDurations[this.currentSegment];
        }

        GameObject g0 = this.ControlPoints[ClampListPos(this.currentSegment - 1)];
        GameObject g1 = this.ControlPoints[this.currentSegment];
        GameObject g2 = this.ControlPoints[ClampListPos(this.currentSegment + 1)];
        GameObject g3 = this.ControlPoints[ClampListPos(this.currentSegment + 2)];

        if (g0 == null || g1 == null || g2 == null || g3 == null)
        {
            Debug.LogWarningFormat("Object [{0}]: one of the control points on CatmullRomSpline is null", this.gameObject.name);
            return;
        }

        Vector3 p0 = g0.transform.position;
        Vector3 p1 = g1.transform.position;
        Vector3 p2 = g2.transform.position;
        Vector3 p3 = g3.transform.position;

        float segmentStartTime = segmentEndTime - this.SegmentDurations[this.currentSegment];
        float t = this.currentTime.Remap(segmentStartTime, segmentEndTime, 0, 1);

        Vector3 p = GetCatmullRomPosition(t, p0, p1, p2, p3);
        this.ObjectToMoveAlongSpline.position = p;
    }

    // ------------------------------------------------------------------------------------- //

    /// <summary>
    /// Display without having to press play
    /// </summary>
    public void OnDrawGizmos()
    {
        if (this.ControlPoints.Length == 0)
        {
            return;
        }
        if (!this.GetComponent<LineRenderer>().enabled)
        {
            return;
        }
        Gizmos.color = Color.white;

        RefreshLinePoints();

        for (int i = 0; i < this.linePoints.Count - 1; i++)
        {
            Gizmos.DrawLine(this.linePoints[i], this.linePoints[i + 1]);
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void RefreshLinePoints()
    {
        this.linePoints.Clear();

        // Draw the Catmull-Rom spline between the points
        for (int i = 0; i < this.ControlPoints.Length; i++)
        {
            // Cant draw between the endpoints
            // Neither do we need to draw from the second to the last endpoint
            // ...if we are not making a looping line
            if ((i == 0 || i == this.ControlPoints.Length - 2 || i == this.ControlPoints.Length - 1) && !this.IsLooping)
            {
                continue;
            }

            GetLinePointsForIndex(i);
        }

        this.LineRenderer.SetPositions(this.linePoints.ToArray());
    }

    // ------------------------------------------------------------------------------------- //

    private void GetLinePointsForIndex(int index)
    {
        var i0 = ClampListPos(index - 1);
        var i1 = index;
        var i2 = ClampListPos(index + 1);
        var i3 = ClampListPos(index + 2);

        if (ControlPoints[i0] == null || ControlPoints[i1] == null ||
            ControlPoints[i2] == null || ControlPoints[i3] == null)
        {
            return;
        }

        //The 4 points we need to form a spline between p1 and p2
        Vector3 p0 = ControlPoints[i0].transform.position;
        Vector3 p1 = ControlPoints[i1].transform.position;
        Vector3 p2 = ControlPoints[i2].transform.position;
        Vector3 p3 = ControlPoints[i3].transform.position;

        // The start position of the line
        Vector3 lastPos = p1;

        this.linePoints.Add(lastPos);

        // How many times should we loop?
        int loops = Mathf.FloorToInt(1f / this.SplineResolution);

        for (int i = 1; i <= loops; i++)
        {
            // Which t position are we at?
            float t = i * this.SplineResolution;

            // Find the coordinate between the end points with a Catmull-Rom spline
            Vector3 p = GetCatmullRomPosition(t, p0, p1, p2, p3);

            this.linePoints.Add(p);
        }
    }

    // ------------------------------------------------------------------------------------- //

    /// <summary>
    /// Clamp the list positions to allow looping
    /// </summary>
    private int ClampListPos(int pos)
    {
        if (pos < 0)
        {
            pos = ControlPoints.Length - 1;
        }

        if (pos > ControlPoints.Length)
        {
            pos = 1;
        }
        else if (pos > ControlPoints.Length - 1)
        {
            pos = 0;
        }

        return pos;
    }

    // ------------------------------------------------------------------------------------- //

    /// <summary>
    /// Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    /// http://www.iquilezles.org/www/articles/minispline/minispline.htm
    /// </summary>
    private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }

    // ------------------------------------------------------------------------------------- //
}
