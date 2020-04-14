using System.Collections;
using System.Collections.Generic;

public class GlobalFunction
{
    static public float newScale(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        float oldRange = oldMax - oldMin;
        float newRangge = newMax - newMin;
        float newValue = (oldValue - oldMin) / oldRange * newRangge + newMin;

        return newValue;
    }
}
