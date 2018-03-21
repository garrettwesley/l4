using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// See http://gizma.com/easing/#l
/// </summary>
public static class EasingUtils
{
    // ------------------------------------------------------------------------------------- //

    public static float EaseInQuad(
        float currentTime,
        float startValue,
        float delta,
        float duration)
    {
        currentTime /= duration;
        float result = delta * currentTime * currentTime + startValue;
        return result;
    }

    // ------------------------------------------------------------------------------------- //

    public static float EaseOutQuad(
        float currentTime,
        float startValue,
        float delta,
        float duration)
    {
        currentTime /= duration;
        float result = -delta * currentTime * (currentTime - 2) + startValue;
        return result;
    }

    // ------------------------------------------------------------------------------------- //
}
