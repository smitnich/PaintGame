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
    public void setColor(Vector3 initPos, Vector3 endPos, Color color, int size)
    {
        float x = initPos.x;
        float y = initPos.y;
        if (x < start.x || x > end.x)
            return;
        if (y < start.y || y > end.y)
            return;
        float xPos = x - start.x;
        float yPos = y - start.y;
        int xPixel = Mathf.RoundToInt(xPos * horizPixels/width);
        int yPixel = Mathf.RoundToInt(yPos * vertPixels / height);
        int xPixelEnd = Mathf.RoundToInt((endPos.x - start.x) * horizPixels / width);
        int yPixelEnd = Mathf.RoundToInt((endPos.y - start.y) * vertPixels / height);
        SetPixelCircle(horizPixels-xPixel, vertPixels-yPixel, size/2, color);
        DrawLine(horizPixels - xPixel, vertPixels - yPixel, horizPixels - xPixelEnd, vertPixels - yPixelEnd, size, color);
    }
    public void SetPixelCircle(int xPos, int yPos, int radius, Color color) {
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
                    setPixel(x, y, color);
                }
            }
        }
    }
    //Determine the radius in terms of pixels of the object with horizontal size of objSize
    public int determineSize(float objSize)
    {
        return Mathf.RoundToInt(objSize * horizPixels / width)*2;
    }
    public void DrawLineY(int xStart, int yStart, int xEnd, int yEnd, int radius, Color color)
    {
        double xSlope = ((double)(xEnd - xStart)/(yEnd - yStart));
        if (double.IsNaN(xSlope) || double.IsInfinity(xSlope))
        {
            xSlope = 0;
        }
        if (yStart > yEnd)
        {
            int temp = yStart;
            yStart = yEnd;
            yEnd = temp;
            temp = xStart;
            xStart = xEnd;
            xEnd = temp;
        }
        for (int i = yStart; i <= yEnd; i++)
        {            
            for (int j = -radius / 2; j <= radius / 2; j++)
            {
              setPixel(Mathf.RoundToInt((float)(xStart + (i - yStart)*xSlope))+j,i, color);
            }
        }
    }
    public void DrawLine(int xStart, int yStart, int xEnd, int yEnd, int radius, Color color)
    {
        double ySlope = ((double)(yEnd - yStart) / (xEnd - xStart));
        if (ySlope > 1 || ySlope < -1)
        {
            DrawLineY(xStart, yStart, xEnd, yEnd, radius, color);
        }
        if (double.IsNaN(ySlope) || double.IsInfinity(ySlope))
        {
            ySlope = 0;
        }
        if (xStart > xEnd)
        {
            int temp = xStart;
            xStart = xEnd;
            xEnd = temp;
            temp = yStart;
            yStart = yEnd;
            yEnd = temp;
        }
        for (int i = xStart; i <= xEnd; i++)
        {
            for (int j = -radius / 2; j <= radius / 2; j++)
            {
                setPixel(i, Mathf.RoundToInt((float)(yStart + (i - xStart) * ySlope))+j, color);
            }
        }
    }
    void setPixel(int x, int y, Color color)
    {
        if (x < 0 || y < 0 || x >= horizPixels || y >= vertPixels)
            return;
        colors[x + y * horizPixels] = color;
    }
}
