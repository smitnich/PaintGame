using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlitColors : MonoBehaviour {
    public int horizPixels = 1024;
    public int vertPixels = 1024;
    public Color firstColor = Color.white;
    Color[] colors;
    Texture2D texture;
    public float width;
    public float height;
    public Vector2 start;
    public Vector2 end;
    bool updateRequired = true;
	// Use this for initialization
	public void Init (Color firstColor, Renderer parentRenderer) {
        Vector3 size = GetComponent<Collider>().bounds.size;
        width = Mathf.RoundToInt(size.x);
        height = Mathf.RoundToInt(size.y);
        start = (transform.position) - size / 2;
        end = (transform.position) + size / 2;
        width = end.x - start.x;
        height = end.y - start.y;
        colors = new Color[horizPixels * vertPixels];
        texture = new Texture2D(horizPixels, vertPixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        GetComponent<Renderer>().material = parentRenderer.material;
        GetComponent<Renderer>().material.mainTexture = texture;
	}
    public void BlitUpdate()
    {
        if (!updateRequired)
            return;
        texture.SetPixels(colors);
        texture.Apply();
        updateRequired = false;
    }
    public void loadPixels(Texture2D input, int x, int y)
    {
        try
        {
            if (input == null)
            {
                for (int i = 0; i < horizPixels * vertPixels; i++)
                    colors[i] = firstColor;
            }
            else
            {
                colors = input.GetPixels(x * horizPixels, y * vertPixels, horizPixels, vertPixels);
            }
            BlitUpdate();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public void setPixel(int x, int y, Color color)
    {
        if (x < 0 || y < 0 || x >= horizPixels || y >= vertPixels)
            return;
        colors[(horizPixels-x-1) + (vertPixels-y-1) * horizPixels] = color;
        updateRequired = true;
    }
    public int[] absorbPixel(int x, int y, int absorbStrength, int[] result)
    {
        float[] tmpChange = { 0f, 0f, 0f };
        int[] change = { 0, 0, 0 };
        if (x < 0 || y < 0 || x >= horizPixels || y >= vertPixels)
            return change;
        Color tmpColor = colors[(horizPixels-x-1) + (vertPixels-y-1) * horizPixels];
        tmpChange[0] = Mathf.Min((float)(absorbStrength / 255f), 1.0f - tmpColor.r);
        tmpChange[1] = Mathf.Min((float)(absorbStrength / 255f), 1.0f - tmpColor.g);
        tmpChange[2] = Mathf.Min((float)(absorbStrength / 255f), 1.0f - tmpColor.b);
        tmpColor.r += tmpChange[0];
        tmpColor.g += tmpChange[1];
        tmpColor.b += tmpChange[2];
        //Since adding colors moves towards white, we should consider the amount of red
        //to be gathered to be the average of the green and blue added
        change[0] = (int) ((tmpChange[1] + tmpChange[2])*255f) / 2;
        change[1] = (int) ((tmpChange[0] + tmpChange[2])*255f) / 2;
        change[2] = (int) ((tmpChange[0] + tmpChange[1])*255f) / 2;
        colors[(horizPixels-x-1) + (vertPixels-y-1) * horizPixels] = tmpColor;
        updateRequired = true;
        for (int i = 0; i < result.Length; i++)
        {
            result[i] += change[i];
        }
        return change;
    }
}
