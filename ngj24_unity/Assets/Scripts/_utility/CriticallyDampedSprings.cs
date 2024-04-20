using System;
using UnityEngine;


[Serializable]
public class VecSpringDamp
{
    [Range(1f, 20.0f)]
    public float Frequency = 2f;
    [Range(0.01f, 20.0f)]
    public float HalfLife = 1f;
    
    private FloatSpringDamp x;
    private FloatSpringDamp y;
    private FloatSpringDamp z;

    public void Init(Vector3 initPos)
    {
        x = new FloatSpringDamp(initPos.x);
        y = new FloatSpringDamp(initPos.y);
        z = new FloatSpringDamp(initPos.z);

        SetFrequency(Frequency);
        SetHalfLife(HalfLife);
    }

    public void ForcePositionOverride(Vector3 pos)
    {
        x.SetPos(pos.x);
        y.SetPos(pos.y);
        z.SetPos(pos.z);
    }

    public Vector3 MoveTowards(Vector3 target, float goalVelocity)
    {
        SetFrequency(Frequency);
        SetHalfLife(HalfLife);

        return new Vector3
        (
            x.MoveTowards(target.x, goalVelocity),
            y.MoveTowards(target.y, goalVelocity),
            z.MoveTowards(target.z, goalVelocity)
        );
    }

    public Vector3 Pos()
    {
        return new Vector3(x.Pos(), y.Pos(), z.Pos());
    }

    public void SetFrequency(float frequency)
    {
        Frequency = frequency;
        x.Frequency = y.Frequency = z.Frequency = Frequency;
    }
    
    public void SetHalfLife(float halfLife)
    {
        HalfLife = halfLife;
        x.HalfLife = y.HalfLife = z.HalfLife = HalfLife;
    }
}

[Serializable]
public class FloatSpringDamp
{
    public float Frequency;
    public float HalfLife;
    
    private float x;
    private float v;

    public FloatSpringDamp(float initPos)
    {
        x = initPos;
    }

    public void SetPos(float pos)
    {
        x = pos;
    }

    public float Pos()
    {
        return x;
    }

    public float MoveTowards(float goal, float goalVelocity)
    {
        damper_spring(goal, goalVelocity, Frequency, HalfLife, Time.fixedDeltaTime);
        return x;
    }
    
    // Good resources:
    // https://theorangeduck.com/page/spring-roll-call#damper
    // https://www.alexisbacot.com/blog/the-art-of-damping
    // https://github.com/AlexisBacot/ArtOfDamping/blob/main/Assets/Scripts/ToolDamper.cs
    
    /// <summary>
    /// Damper spring
    /// </summary>
    /// <param name="frequency">how many oscillations are done</param>
    /// <param name="halflife">how quick the goal is reached</param>
    public void damper_spring(float x_goal, float v_goal, float frequency, float halflife, float dt)
    {
        float g = x_goal;
        float q = v_goal;
        float s = frequency_to_stiffness(frequency);
        float d = halflife_to_damping(halflife);
        float c = g + (d * q) / (s + Mathf.Epsilon);
        float y = d / 2.0f;

      //  if (Mathf.Abs(s - (d * d) / 4.0f) < Mathf.Epsilon) // Critically Damped
        {
            float j0 = x - c;
            float j1 = v + j0 * y;

            float eydt = fast_negexp(y * dt);

            x = j0 * eydt + dt * j1 * eydt + c;
            v = -y * j0 * eydt - y * dt * j1 * eydt + j1 * eydt;
        }
        // else if (s - (d * d) / 4.0f > 0.0) // Under Damped
        // {
        //     float w = Mathf.Sqrt(s - (d * d) / 4.0f);
        //     float j = Mathf.Sqrt(squaref(v + y * (x - c)) / (w * w + Mathf.Epsilon) + squaref(x - c));
        //     float p = Mathf.Atan((v + (x - c) * y) / (-(x - c) * w + Mathf.Epsilon));
        //
        //     j = (x - c) > 0.0f ? j : -j;
        //
        //     float eydt = fast_negexp(y * dt);
        //
        //     x = j * eydt * Mathf.Cos(w * dt + p) + c;
        //     v = -y * j * eydt * Mathf.Cos(w * dt + p) - w * j * eydt * Mathf.Sin(w * dt + p);
        // }
        // else if (s - (d * d) / 4.0f < 0.0) // Over Damped
        // {
        //     float y0 = (d + Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
        //     float y1 = (d - Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
        //     float j1 = (c * y0 - x * y0 - v) / (y1 - y0);
        //     float j0 = x - j1 - c;
        //
        //     float ey0dt = fast_negexp(y0 * dt);
        //     float ey1dt = fast_negexp(y1 * dt);
        //
        //     x = j0 * ey0dt + j1 * ey1dt + c;
        //     v = -y0 * j0 * ey0dt - y1 * j1 * ey1dt;
        // }
    }

    //--------------------------------------------------------------------
    private static float frequency_to_stiffness(float frequency)
    {
        return squaref(2.0f * Mathf.PI * frequency);
    }

    //--------------------------------------------------------------------
    private static float stiffness_to_frequency(float stiffness)
    {
        return Mathf.Sqrt(stiffness) / (2.0f * Mathf.PI);
    }

    //--------------------------------------------------------------------
    private static float halflife_to_damping(float halflife)
    {
        return (4.0f * 0.69314718056f) / (halflife + Mathf.Epsilon);
    }

    //--------------------------------------------------------------------
    private static float damping_to_halflife(float damping)
    {
        return (4.0f * 0.69314718056f) / (damping + Mathf.Epsilon);
    }
    
    //--------------------------------------------------------------------
    private static float fast_negexp(float x)
    {
        return 1.0f / (1.0f + x + 0.48f * x * x + 0.235f * x * x * x);
    }
    
    //--------------------------------------------------------------------
    private static float squaref(float x) { return x * x; }
}