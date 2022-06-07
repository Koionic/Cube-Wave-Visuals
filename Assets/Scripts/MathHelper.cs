using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    
    public static float Map(float t, float minInput, float maxInput, float minOutput, float maxOutput)
    {
        return (t - minInput) * (maxOutput - minOutput) / (maxInput - minInput) + minOutput;
    }
}
