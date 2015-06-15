using UnityEngine;
using System.Collections;

public class BlitColors : MonoBehaviour {
    public int horizPixels = 1024;
    public int vertPixels = 1024;
    public Color firstColor = Color.white;
    Color[] colors;
    Texture2D texture;
    float width;
    float height;
    Vector2 start;
    Vector2 end;
    BlitColors pixelScript;
	// Use this for initialization
	void Start () {
        Vector3 size = GetComponent<Collider>().bounds.size;
        width = Mathf.RoundToInt(size.x);
        height = Mathf.RoundToInt(size.y);
        Vector3 test = transform.position;
        start = (transform.position) - size / 2;
        end = (transform.position) + size / 2;
        width = end.x - start.x;
        height = end.y - start.y;
        GameObject floor = GameObject.Find("Floor");
        pixelScript = floor.GetComponent<BlitColors>();
        colors = new Color[horizPixels * vertPixels];
        for (int i = 0; i < horizPixels * vertPixels; i++)
            colors[i] = firstColor;
        texture = new Texture2D(horizPixels, vertPixels);
        GetComponent<Renderer>().material.mainTexture = texture;
	}
    // Update is called once per frame
    void LateUpdate()
    {
        texture.SetPixels(colors);
        texture.Apply();
    }
    public void setColor(float x, float y, Color color, int size)
    {
        if (x < start.x || x > end.x)
            return;
        if (y < start.y || y > end.y)
            return;
        float xPos = x - start.x;
        float yPos = y - start.y;
        int xPixel = Mathf.RoundToInt(xPos * horizPixels/width);
        int yPixel = Mathf.RoundToInt(yPos * vertPixels / height);
        pixelScript.SetPixel(horizPixels-xPixel, vertPixels-yPixel, size, color);
    }
    public void SetPixel(int xPos, int yPos, int radius, Color color) {
        int squaredRadius = radius*radius;
        for (int x = xPos - radius; x <= xPos + radius; x++)
        {
            for (int y = yPos - radius; y <= yPos + radius; y++)
            {
                if (((x + y * horizPixels) >= (horizPixels * vertPixels)) || (x+y*horizPixels < 0))
                    continue;
                int xDist = Mathf.Abs(x-xPos);
                int yDist = Mathf.Abs(y-yPos);
                if ((xDist * xDist) + (yDist * yDist) <= squaredRadius)
                {
                    colors[x+y*horizPixels] = color;
                }
            }
        }
    }
}
