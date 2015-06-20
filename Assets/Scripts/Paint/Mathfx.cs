//This library is imported from http://wiki.unity3d.com/index.php?title=Mathfx

using UnityEngine;
using System.Collections;

public class Mathfx : MonoBehaviour 
{

    public static float Hermite(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }

    public static float Coserp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
    }

    public static float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1 - value, 2.2f) + value) * (1 + (1.2f * (1 - value)));
        return start + (end - start) * value;
    }

    public static float SmoothStep(float x, float min, float max)
    {
        x = Mathf.Clamp(x, min, max);
        var v1 = (x - min) / (max - min);
        var v2 = (x - min) / (max - min);
        return -2 * v1 * v1 * v1 + 3 * v2 * v2;
    }

    public static float Lerp(float start, float end, float value)
    {
        return ((1.0f - value) * start) + (value * end);
    }

    public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        var lineDirection = Vector3.Normalize(lineEnd - lineStart);
        var closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        return lineStart + (closestPoint * lineDirection);
    }

    public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        var fullDirection = lineEnd - lineStart;
        var lineDirection = Vector3.Normalize(fullDirection);
        var closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
    }

    public static Vector2 NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    {
        var fullDirection = lineEnd - lineStart;
        var lineDirection = Normalize(fullDirection);
        var closestPoint = Vector2.Dot((point - lineStart), lineDirection) / Vector2.Dot(lineDirection, lineDirection);
        return lineStart + (Mathf.Clamp(closestPoint, 0.0f, fullDirection.magnitude) * lineDirection);
    }



    public static float Bounce(float x)
    {
        return Mathf.Abs(Mathf.Sin(6.28f * (x + 1) * (x + 1)) * (1 - x));
    }

    // test for value that is near specified float (due to floating point inprecision)
    // all thanks to Opless for this!
    public static bool Approx(float val, float about, float range)
    {
        return ((Mathf.Abs(val - about) < range));
    }

    // test if a Vector3 is close to another Vector3 (due to floating point inprecision)
    // compares the square of the distance to the square of the range as this
    // avoids calculating a square root which is much slower than squaring the range
    public static bool Approx(Vector3 val, Vector3 about, float range)
    {
        return ((val - about).sqrMagnitude < range * range);
    }

    public static float GaussFalloff(float distance, float inRadius)
    {
        return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
    }
    // CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
    // This is useful when interpolating eulerAngles and the object
    // crosses the 0/360 boundary.  The standard Lerp function causes the object
    // to rotate in the wrong direction and looks stupid. Clerp fixes that.
    public static float Clerp(float start, float end, float value)
    {
        var min = 0.0f;
        var max = 360.0f;
        var half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
        var retval = 0.0f;
        var diff = 0.0f;

        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else retval = start + (end - start) * value;

        return retval;
    }


    //======= NEW =========//


    public static Vector2 RotateVector(Vector2 vector, float rad)
    {
        rad *= Mathf.Deg2Rad;
        var res = new Vector2((vector.x * Mathf.Cos(rad)) - (vector.y * Mathf.Sin(rad)), (vector.x * Mathf.Sin(rad)) + (vector.y * Mathf.Cos(rad)));
        return res;
    }

    public static Vector2 IntersectPoint(Vector2 start1, Vector2 start2, Vector2 dir1, Vector2 dir2)
    {
        if (dir1.x == dir2.x)
        {
            return Vector2.zero;
        }

        var h1 = dir1.y / dir1.x;
        var h2 = dir2.y / dir2.x;

        if (h1 == h2)
        {
            return Vector2.zero;
        }

        var line1 = new Vector2(h1, start1.y - start1.x * h1);
        var line2 = new Vector2(h2, start2.y - start2.x * h2);

        var y1 = line2.y - line1.y;
        var x1 = line1.x - line2.x;

        var x2 = y1 / x1;

        var y2 = line1.x * x2 + line1.y;
        return new Vector2(x2, y2);
    }

    public static Vector2 ThreePointCircle(Vector2 a1, Vector2 a2, Vector2 a3)
    {
        var dir = a2 - a1;
        dir /= 2;
        var b1 = a1 + dir;
        dir = RotateVector(dir, 90);
        var l1 = dir;

        dir = a3 - a2;
        dir /= 2;
        var b2 = a2 + dir;
        dir = RotateVector(dir, 90);
        var l2 = dir;
        var p = IntersectPoint(b1, b2, l1, l2);
        return p;
    }

    public static Vector2 NearestPointOnCircle(Vector2 p, Vector2 center, float w)
    {
        Vector2 dir = p - center;
        dir = Normalize(dir);
        dir *= w;
        return center + dir;
    }

    public static Vector2 Normalize(Vector2 p)
    {
        float mag = p.magnitude;
        return p / mag;
    }


}
