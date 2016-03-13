using UnityEngine;

//This class is responsible for creating an array of floors to blit colors onto
//This is necessary because needing to update a 1024*1024 takes way too long
//to run at a decent framerate
//By splitting the floor into multiple component, we only need to update the
//texture of portions of the floor that were rendered to this frame
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
    Renderer rend;
	/// <summary>
    /// Initialize the floorManager object and create the necessary floor
    /// objects
    /// </summary>
    void Start () {
        rend = GetComponent<Renderer>();
        startX = gameObject.transform.position.x-(rend.bounds.size.x)/2;
        startY = gameObject.transform.position.y-(rend.bounds.size.y)/2;
        start.x = startX;
        start.y = startY;
        endX = startX + rend.bounds.size.x;
        endY = startY + rend.bounds.size.y;
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
    /// <summary>
    /// Call BlitUpdate on all floors
    /// </summary>
    void LateUpdate()
    {
        foreach (GameObject obj in floors) {
            obj.GetComponent<BlitColors>().BlitUpdate();
        }
    }
    /// <summary>
    /// Set an individual pixels color
    /// </summary>
    /// <param name="x">X position to set</param>
    /// <param name="y">Y position to set</param>
    /// <param name="color">Color to set to</param>
    void SetPixel(int x, int y, Color color)
    {
        int floorX = x / planeWidthPixels;
        int floorY = y / planeHeightPixels;
        if (floorX >= floorsPerRow || floorY >= floorsPerColumn || floorX < 0 || floorY < 0)
            return;
        GameObject floor = floors[floorsPerRow - 1 - floorX, floorsPerColumn - 1 - floorY];
        if (floor != null)
            floor.GetComponent<BlitColors>().SetPixel(x % planeWidthPixels, y % planeHeightPixels, color);
    }
    /// <summary>
    /// Remove color from a given pixel
    /// </summary>
    /// <param name="x">X position to modify</param>
    /// <param name="y">Y position to modify</param>
    /// <param name="absorbStrength">Maximum amount of color to remove</param>
    /// <param name="result">An array containing the amount of Red, Green, and Blue removed</param>
    /// <returns></returns>
    int[] AbsorbPixel(int x, int y, int absorbStrength, int[] result)
    {
        int[] returnVal = {0,0,0};
        int floorX = x / planeWidthPixels;
        int floorY = y / planeHeightPixels;
        if (floorX >= floorsPerRow || floorY >= floorsPerColumn || floorX < 0 || floorY < 0)
            return returnVal;
        GameObject floor = floors[floorsPerRow-1-floorX,floorsPerColumn-1-floorY];
        if (floor != null)
            return floor.GetComponent<BlitColors>().AbsorbPixel(x % planeWidthPixels, y % planeHeightPixels, absorbStrength, result);
        else
            return returnVal;
    }
    /// <summary>
    /// Draw a line between two points, as well as a circle at the initial location
    /// </summary>
    /// <param name="initPos">Position to start drawing from</param>
    /// <param name="endPos">Position to stop drawing at</param>
    /// <param name="color">Color to set the modified pixels to</param>
    /// <param name="size">Size of the line and circle</param>
    public void SetColor(Vector3 initPos, Vector3 endPos, Color color, int size)
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
    /// <summary>
    /// Wrapper function for calling AbsorbPixelPerimeter
    /// </summary>
    /// <param name="pos">Position to absorb at</param>
    /// <param name="radius">Distance from pos to absorb at</param>
    /// <param name="absorbStrength">Maximum amount of color allowed to be removed</param>
    /// <returns>An array containing the amount of Red, Green, and Blue removed</returns>
    public int[] AbsorbCirclePerimeter(Vector3 pos, int radius, int absorbStrength)
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
    /// <summary>
    /// A wrapper function for AbsorbPixelCircle
    /// </summary>
    /// <param name="pos">Position to absorb at</param>
    /// <param name="radius">Distance from pos to absorb within</param>
    /// <param name="absorbStrength">Maximum amount of color allowed to be removed</param>
    /// <returns>An array containing the amount of Red, Green, and Blue removed</returns>
    public int[] AbsorbCircle(Vector3 pos, int radius, int absorbStrength)
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
    /// <summary>
    /// Remove color within a circle
    /// </summary>
    /// <param name="xPos">X position of the center of the circle</param>
    /// <param name="yPos">Y position of the center of the circle</param>
    /// <param name="radius">Radius to absorb within</param>
    /// <param name="absorbStrength">Maximum amount of color allowed to be removed</param>
    /// <returns>An array containing the amount of Red, Green, and Blue removed</returns>
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
                    AbsorbPixel(x, y, absorbStrength, returnVal);
                }
            }
        }
        return returnVal;
    }
    /// <summary>
    /// Remove color along the perimeter of a circle
    /// </summary>
    /// <param name="xPos">X position of the center of the circle</param>
    /// <param name="yPos">Y position of the center of the circle</param>
    /// <param name="radius">Distance to absorb at</param>
    /// <param name="absorbStrength">Maximum amount of color allowed to be removed</param>
    /// <returns>An array containing the amount of Red, Green, and Blue removed</returns>
    public int[] AbsorbPixelPerimeter(int x0, int y0, int radius, int absorbStrength)
    {
        int x = radius;
        int y = 0;
        int decisionOver2 = 1 - x;   // Decision criterion divided by 2 evaluated at x=r, y=0
        int[] result = {0,0,0};
        while (x >= y)
        {
            //Some pixels get missed; set the pixels directly next to them in order to avoid this
            for (int xOff = -1; xOff <= 1; xOff++)
            {
                AbsorbPixel(x + x0 - xOff, y + y0, absorbStrength, result);
                AbsorbPixel(y + x0 - xOff, x + y0, absorbStrength, result);
                AbsorbPixel(-x + x0 - xOff, y + y0, absorbStrength, result);
                AbsorbPixel(-y + x0 - xOff, x + y0, absorbStrength, result);
                AbsorbPixel(-x + x0 - xOff, -y + y0, absorbStrength, result);
                AbsorbPixel(-y + x0 - xOff, -x + y0, absorbStrength, result);
                AbsorbPixel(x + x0 - xOff, -y + y0, absorbStrength, result);
                AbsorbPixel(y + x0 - xOff, -x + y0, absorbStrength, result);
            }
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
    /// <summary>
    /// Set the color of pixels within the radius of a circle
    /// </summary>
    /// <param name="xPos">X position of the center of the circle</param>
    /// <param name="yPos">Y position of the center of the circle</param>
    /// <param name="radius">Radius to set within</param>
    /// <param name="color">Color to set to</param>
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
                   SetPixel(x, y, color);
                }
            }
        }
    }
    /// <summary>
    // Determine the radius in terms of pixels of the object with horizontal size of objSize
    // This scales the size of objects to the size of pixels
    /// </summary>
    /// <param name="objSize"></param>
    /// <returns></returns>
    public int determineSize(float objSize)
    {
        return Mathf.RoundToInt(objSize * sizeX / width) * 2;
    }
    /// <summary>
    /// Draw a line with a vertical slope between two points
    /// </summary>
    /// <param name="xStart">X position to start drawing at</param>
    /// <param name="yStart">Y position to start drawing at</param>
    /// <param name="xEnd">X position to stop drawing at</param>
    /// <param name="yEnd">Y position to stop drawing at</param>
    /// <param name="radius">Width of the line</param>
    /// <param name="color">The color of the line</param>
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
                SetPixel(Mathf.RoundToInt((float)(xStart + (i - yStart) * xSlope)) + j, i, color);
            }
        }
    }
    /// <summary>
    /// Draw a line between two points
    /// Calls the helper function DrawLineY if the line is vertical
    /// </summary>
    /// <param name="xStart"></param>
    /// <param name="yStart"></param>
    /// <param name="xEnd"></param>
    /// <param name="yEnd"></param>
    /// <param name="radius"></param>
    /// <param name="color"></param>
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
                SetPixel(i, Mathf.RoundToInt((float)(yStart + (i - xStart) * ySlope)) + j, color);
            }
        }
    }
    Vector2 PixelToGameCoords(int x, int y)
    {
        float widthPerPixel = truePlaneWidth/planeWidthPixels;
        float heightPerPixel = truePlaneHeight / planeHeightPixels;
        //return new Vector2(startX + widthPerPixel * x, startY + heightPerPixel * y);
        return new Vector2(endX - widthPerPixel * x, endY - heightPerPixel * y);
    }
    public Vector2 GameCoordsToPixel(float x, float y)
    {
        float xPos = x - start.x;
        float yPos = y - start.y;
        int xPixel = Mathf.RoundToInt(xPos * sizeX / width);
        int yPixel = Mathf.RoundToInt(yPos * sizeY / height);
        return new Vector2(sizeX - xPixel, sizeY - yPixel);
    }
    public void FillRaycast(Vector2 initPos, Vector2 endPos, GameObject obj)
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
        FillRaycastSquare(sizeX - xPixelEnd, sizeY - yPixelEnd, sizeX - xPixel, sizeY - yPixel, obj);
    }
    private bool CheckObjectRaycast(int pixelX, int pixelY, GameObject obj)
    {
        Vector2 pos = PixelToGameCoords(pixelX, pixelY);
        Collider2D coll = obj.GetComponent<Collider2D>();
        if (coll.OverlapPoint(pos))
        {
            return true;
        }
        return false;
    }
    public void FillRaycastSquare(int xStart, int yStart, int xEnd, int yEnd, GameObject obj)
    {
        for (int x = xStart; x <= xEnd; x++)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                if (CheckObjectRaycast(x, y, obj))
                    SetPixel(x, y, obj.GetComponent<SetColor>().color);
            }
        }
    }
    /// <summary>
    /// Create a basic, flat floor using the built in Plane Primitive
    /// </summary>
    /// <param name="x">The X location of the floor within the FloorManager</param>
    /// <param name="y">The X location of the floor within the FloorManager</param>
    /// <returns>A plane with the proper pixels set for that portion of the floor</returns>
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
        script.Init(Color.white, rend);
        script.loadPixels(baseImage, x, y);
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        for (int i = 11; i < 110; i++)
            verts[i].y += 1;
        mesh.vertices = verts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return obj;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
