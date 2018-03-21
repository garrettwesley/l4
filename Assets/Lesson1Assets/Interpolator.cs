using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Interpolator
{
    private float startValue;
    private float endValue;
    private float duration;

    public float CurrentValue
    {
        get;
        private set;
    }

    // ------------------------------------------------------------------------------------- //

    public void SetParameters(float startValue, float endValue, float duration)
    {
        this.startValue = startValue;
        this.CurrentValue = this.startValue;
        this.endValue = endValue;
        this.duration = duration;
    }

    // ------------------------------------------------------------------------------------- //

    public void Restart()
    {
        this.CurrentValue = this.startValue;
    }

    // ------------------------------------------------------------------------------------- //

    public void Update(float deltaTime)
    {
        float deltaValue = deltaTime * (this.endValue - this.startValue) / this.duration;
        this.CurrentValue += deltaValue;
    }

    // ------------------------------------------------------------------------------------- //
}
