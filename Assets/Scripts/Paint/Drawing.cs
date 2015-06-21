using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Drawing
{

    public enum Samples
    {
        None,
        Samples2,
        Samples4,
        Samples8,
        Samples16,
        Samples32,
        RotatedDisc
    }

    public static Samples numSamples = Samples.Samples4;

    public static Texture2D PaintLine(Vector2 from, Vector2 to, float rad, Color col, float hardness, Texture2D tex)
    {
        var extent = rad;
        var stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - extent, 0, tex.height);
        var stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - extent, 0, tex.width);
        var endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + extent, 0, tex.height);
        var endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + extent, 0, tex.width);

        var lengthX = endX - stX;
        var lengthY = endY - stY;
        var sqrRad2 = (rad + 1) * (rad + 1);
        Color[] pixels = tex.GetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, 0);
        var start = new Vector2(stX, stY);
        for (int y = 0; y < (int)lengthY; y++)
        {
            for (int x = 0; x < (int)lengthX; x++)
            {
                var p = new Vector2(x, y) + start;
                var center = p + new Vector2(0.5f, 0.5f);
                float dist = (center - Mathfx.NearestPointStrict(from, to, center)).sqrMagnitude;
                if (dist > sqrRad2)
                {
                    continue;
                }
                dist = Mathfx.GaussFalloff(Mathf.Sqrt(dist), rad) * hardness;
                Color c;
                if (dist > 0)
                {
                    c = Color.Lerp(pixels[y * (int)lengthX + x], col, dist);
                }
                else
                {
                    c = pixels[y * (int)lengthX + x];
                }

                pixels[y * (int)lengthX + x] = c;
            }
        }
        tex.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
        return tex;
    }
}
