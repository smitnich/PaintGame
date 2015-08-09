using UnityEngine;
using System.Collections;

public class FloorManager : MonoBehaviour {
    GameObject[,] floors;
    public int floorsPerRow = 8;
    int floorsPerColumn = 4;
    public int sizeX = 2048;
    public int sizeY = 1024;
    int planeWidthPixels;
    int planeHeightPixels;
    float truePlaneWidth;
    float truePlaneHeight;
    float startX, startY, endX, endY;
    float width;
    float height;
    Vector2 start;
    Vector2 end;
    Texture2D baseImage;
    Renderer renderer;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        startX = gameObject.transform.position.x-(renderer.bounds.size.x)/2;
        startY = gameObject.transform.position.y-(renderer.bounds.size.y)/2;
        start.x = startX;
        start.y = startY;
        endX = startX + renderer.bounds.size.x;
        endY = startY + renderer.bounds.size.y;
        end.x = endX;
        end.y = endY;
        floorsPerColumn = floorsPerRow / 2;
        planeWidthPixels  = sizeX / floorsPerRow;
        planeHeightPixels = sizeY / floorsPerColumn;
        truePlaneWidth = Mathf.Abs((endX - startX) / floorsPerRow);
        truePlaneHeight = Mathf.Abs((endY - startY) / floorsPerColumn);
        width = endX - startX;
        height = endY - startY;
        baseImage = Resources.Load("testImage", typeof(Texture2D)) as Texture2D;
        floors = new GameObject[floorsPerRow, floorsPerColumn];
        for (int i = 0; i < floorsPerRow; i++)
            for (int j = 0; j < floorsPerColumn; j++)
                floors[i, j] = createFloor(i, j);
	}
    void LateUpdate()
    {
        foreach (GameObject obj in floors) {
            obj.GetComponent<BlitColors>().BlitUpdate();
        }
    }
    void setPixel(int x, int y, Color color)
    {
        int floorX = x / planeWidthPixels;
        int floorY = y / planeHeightPixels;
        if (floorX >= floorsPerRow || floorY >= floorsPerColumn || floorX < 0 || floorY < 0)
            return;
        GameObject floor = floors[floorsPerRow - 1 - floorX, floorsPerColumn - 1 - floorY];
        if (floor != null)
            floor.GetComponent<BlitColors>().setPixel(x % planeWidthPixels, y % planeHeightPixels, color);
    }
    int[] absorbPixel(int x, int y, int absorbStrength, int[] result)
    {
        int[] returnVal = {0,0,0};
        int floorX = x / planeWidthPixels;
        int floorY = y / planeHeightPixels;
        if (floorX >= floorsPerRow || floorY >= floorsPerColumn || floorX < 0 || floorY < 0)
            return returnVal;
        GameObject floor = floors[floorsPerRow-1-floorX,floorsPerColumn-1-floorY];
        if (floor != null)
            return floor.GetComponent<BlitColors>().absorbPixel(x % planeWidthPixels, y % planeHeightPixels, absorbStrength, result);
        else
            return returnVal;
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
        int xPixel = Mathf.RoundToInt(xPos * sizeX / width);
        int yPixel = Mathf.RoundToInt(yPos * sizeY / height);
        int xPixelEnd = Mathf.RoundToInt((endPos.x - start.x) * sizeX / width);
        int yPixelEnd = Mathf.RoundToInt((endPos.y - start.y) * sizeY / height);
        SetPixelCircle(sizeX - xPixel, sizeY - yPixel, size / 2, color);
        DrawLine(sizeX - xPixel, sizeY - yPixel, sizeX - xPixelEnd, sizeY - yPixelEnd, size, color);
    }
    public int[] absorbCircleRadius(Vector3 pos, int radius, int absorbStrength)
    {
        int[] returnVal = {0,0,0};
        float x = pos.x;
        float y = pos.y;
        if (x < start.x || x > end.x)
            return returnVal;
        if (y < start.y || y > end.y)
            return returnVal;
        float xPos = x - start.x;
        float yPos = y - start.y;
        int xPixel = Mathf.RoundToInt(xPos * sizeX / width);
        int yPixel = Mathf.RoundToInt(yPos * sizeY / height);
        return AbsorbPixelPerimeter(sizeX - xPixel, sizeY - yPixel, radius, absorbStrength);
    }
    public int[] absorbCircle(Vector3 pos, int radius, int absorbStrength)
    {
        int[] returnVal = { 0, 0, 0 };
        float x = pos.x;
        float y = pos.y;
        if (x < start.x || x > end.x)
            return returnVal;
        if (y < start.y || y > end.y)
            return returnVal;
        float xPos = x - start.x;
        float yPos = y - start.y;
        int xPixel = Mathf.RoundToInt(xPos * sizeX / width);
        int yPixel = Mathf.RoundToInt(yPos * sizeY / height);
        return AbsorbPixelCircle(sizeX - xPixel, sizeY - yPixel, radius, absorbStrength);
    }
    public int[] AbsorbPixelCircle(int xPos, int yPos, int radius, int absorbStrength)
    {
        int[] returnVal = { 0, 0, 0 };
        int squaredRadius = radius * radius;
        for (int x = xPos - radius; x <= xPos + radius; x++)
        {
            for (int y = yPos - radius; y <= yPos + radius; y++)
            {
                if (((x + y * sizeX) >= (sizeX * sizeY)) || (x + y * sizeX < 0))
                    continue;
                int xDist = Mathf.Abs(x - xPos);
                int yDist = Mathf.Abs(y - yPos);
                if ((xDist * xDist) + (yDist * yDist) <= squaredRadius)
                {
                    absorbPixel(x, y, absorbStrength, returnVal);
                }
            }
        }
        return returnVal;
    }
    public int[] AbsorbPixelPerimeter(int x0, int y0, int radius, int absorbStrength)
    {
        int x = radius;
        int y = 0;
        int decisionOver2 = 1 - x;   // Decision criterion divided by 2 evaluated at x=r, y=0
        int[] result = {0,0,0};
        while (x >= y)
        {
            absorbPixel(x + x0, y + y0, absorbStrength, result);
            absorbPixel(y + x0, x + y0, absorbStrength, result);
            absorbPixel(-x + x0, y + y0, absorbStrength, result);
            absorbPixel(-y + x0, x + y0, absorbStrength, result);
            absorbPixel(-x + x0, -y + y0, absorbStrength, result);
            absorbPixel(-y + x0, -x + y0, absorbStrength, result);
            absorbPixel(x + x0, -y + y0, absorbStrength, result);
            absorbPixel(y + x0, -x + y0, absorbStrength, result);
            y++;
            if (decisionOver2 <= 0)
            {
                decisionOver2 += 2 * y + 1;   // Change in decision criterion for y -> y+1
            }
            else
            {
                x--;
                decisionOver2 += 2 * (y - x) + 1;   // Change for y -> y+1, x -> x-1
            }
        }
        return result; 
    }
    public void SetPixelCircle(int xPos, int yPos, int radius, Color color)
    {
        int squaredRadius = radius * radius;
        for (int x = xPos - radius; x <= xPos + radius; x++)
        {
            for (int y = yPos - radius; y <= yPos + radius; y++)
            {
                if (((x + y * sizeX) >= (sizeX * sizeY)) || (x + y * sizeX < 0))
                    continue;
                int xDist = Mathf.Abs(x - xPos);
                int yDist = Mathf.Abs(y - yPos);
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
        return Mathf.RoundToInt(objSize * sizeX / width) * 2;
    }
    public void DrawLineY(int xStart, int yStart, int xEnd, int yEnd, int radius, Color color)
    {
        double xSlope = ((double)(xEnd - xStart) / (yEnd - yStart));
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
                setPixel(Mathf.RoundToInt((float)(xStart + (i - yStart) * xSlope)) + j, i, color);
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
                setPixel(i, Mathf.RoundToInt((float)(yStart + (i - xStart) * ySlope)) + j, color);
            }
        }
    }
    GameObject createFloor(int x, int y)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        obj.transform.position = new Vector3(x*truePlaneWidth+startX+truePlaneWidth/2, y*truePlaneHeight+startY+truePlaneHeight/2, gameObject.transform.position.z-1);
        //Need to scale the y axis to the current z axis, since we'll be rotation it later
        Vector3 scale = new Vector3(truePlaneWidth / obj.GetComponent<Renderer>().bounds.size.x, 1, truePlaneHeight/obj.GetComponent<Renderer>().bounds.size.z);
        obj.transform.localRotation = gameObject.transform.localRotation;
        obj.transform.localScale = scale;
        BlitColors script = obj.AddComponent<BlitColors>();
        script.horizPixels = planeWidthPixels;
        script.vertPixels = planeHeightPixels;
        script.Init(Color.white, renderer);
        script.loadPixels(baseImage, x, y);
        return obj;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
