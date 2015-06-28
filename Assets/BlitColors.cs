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
    bool updateRequired = false;
	// Use this for initialization
	public void Init (Color firstColor, Renderer parentRenderer) {
        Vector3 size = GetComponent<Collider>().bounds.size;
        width = Mathf.RoundToInt(size.x);
        height = Mathf.RoundToInt(size.y);
        Vector3 test = transform.position;
        start = (transform.position) - size / 2;
        end = (transform.position) + size / 2;
        width = end.x - start.x;
        height = end.y - start.y;
        colors = new Color[horizPixels * vertPixels];
        for (int i = 0; i < horizPixels * vertPixels; i++)
            colors[i] = firstColor;
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
    public void setPixel(int x, int y, Color color)
    {
        if (x < 0 || y < 0 || x >= horizPixels || y >= vertPixels)
            return;
        colors[x + y * horizPixels] = color;
        updateRequired = true;
    }
    public void absorbPixel(int x, int y, int absorbStrength)
    {
        if (x < 0 || y < 0 || x >= horizPixels || y >= vertPixels)
            return;
        Color tmpColor = colors[x + y * horizPixels];
        tmpColor.r += Mathf.Min((float)(absorbStrength / 255f), 1.0f - tmpColor.r);
        tmpColor.g += Mathf.Min((float)(absorbStrength / 255f), 1.0f - tmpColor.g);
        tmpColor.b += Mathf.Min((float)(absorbStrength / 255f), 1.0f - tmpColor.b);
        colors[x + y * horizPixels] = tmpColor;
        updateRequired = true;
    }
}
