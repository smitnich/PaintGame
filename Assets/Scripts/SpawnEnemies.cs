using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpawnEnemies : MonoBehaviour {

    [System.Serializable]
    public class SpawnObject : PropertyDrawer
    {
        // The time it takes to spawn one object
        public int spawnRate = 4000;
        // The number of objects to be spawned in one batch
        public int spawnNumber = 10;
        // The delay between obejcts before spawning another
        public int spawnDelay = 100;
        // The gameobject to be spawned.
        public GameObject toSpawn = null;
    }
    private SpawnObject currentlySpawning = null;
    public SpawnObject[] spawnObjects = { null };
    public GameObject spawnWithin = null;
    public float speed = 5.0f;
    LineRenderer lineRend;
    float maxX = 0;
    float minX = 0;
    float maxY = 0;
    float minY = 0;
    Vector2[] points;
    Vector2 offset;
    int spawnCount = 0;
    int drawCount = 0;
    GameObject lineRendParent;
    enum State
    {
        moving,
        drawing,
        waiting,
        pickLocation
    }
    State currentState = State.pickLocation;
    Vector3 spawnAt;
    Vector2 moveTo;
    GameObject player;
    public float minDistanceFromPlayer = 30.0f;

    // Use this for initialization
    void Start () {
        if (spawnWithin == null) {
            spawnWithin = GameObject.Find("Floor");
        }
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        Bounds b = spawnWithin.GetComponent<Renderer>().bounds;
        maxX = b.max.x;
        maxY = b.max.y;
        minY = b.min.y;
        minX = b.min.x;
        lineRendParent = new GameObject();
        lineRend = lineRendParent.AddComponent<LineRenderer>();
        lineRend.materials[0] = GetComponent<Material>();
        lineRend.SetColors(Color.black, Color.black);
        lineRend.SetWidth(0.075f, 0.075f);
    }
	// Get a random location within the range of the floor and at least minDistanceFromPlayer units
    // away
    Vector3 GetRandomLocation()
    {
        Vector3 location;
        Vector3 playerLocation = player.transform.position;
        // Make sure that we don't get stuck in an infinite loop if not location is possible
        for (int i = 0; i < 100; i++)
        {
            location = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), -10);
            if (Vector3.Distance(location, playerLocation) > minDistanceFromPlayer)
            {
                return location;
            }
        }
        return new Vector3(0, 0, 0);
    }
    // Create a new instance of the object that we are currently spawning
    void SpawnAnObject()
    {
        if (spawnCount++ < currentlySpawning.spawnNumber)
        {
            GameObject newObj = Instantiate(currentlySpawning.toSpawn);
            newObj.transform.position = spawnAt;
        }
    }
    void Update () {
        switch(currentState)
        {
            case State.drawing:
                // Move to our next target location
                transform.position = Vector3.MoveTowards(transform.position, moveTo + points[drawCount % points.Length], speed * Time.deltaTime);
                // If we're at that location, we have completed drawing another line
                if (Vector3.Distance(transform.position, moveTo + points[drawCount % points.Length]) == 0.00f)
                {
                    // If we've drawn all of the lines, we have finished creating another object
                    if (drawCount >= points.Length)
                    {
                        SpawnAnObject();
                        // If we've spawned all of the objects for this set, we are done
                        if (spawnCount >= currentlySpawning.spawnNumber)
                        {
                            currentState = State.pickLocation;
                            lineRend.SetVertexCount(0);
                            drawCount = 0;
                        }
                        drawCount = 0;
                    }
                    else
                    {
                        // Move on to the next position to draw to
                        drawCount++;
                        lineRend.SetVertexCount(drawCount + 1);
                        lineRend.SetPosition(drawCount, points[drawCount % points.Length]);
                    }
                }
                // Otherwise update the end of the line to the new position of the spawner
                else if (drawCount < points.Length)
                {
                    lineRend.SetPosition(drawCount, points[drawCount]);
                }
                break;
            case State.moving:
                // Move towards our target location so we can begin spawning objects at it
                transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, moveTo) == 0.0f)
                {
                    currentState = State.drawing;
                    CalcDrawingSpeed();
                    lineRend.SetVertexCount(1);
                    lineRend.SetPosition(0, points[0]);
                    drawCount = 0;
                    lineRend.useWorldSpace = false;
                    // Move our line renderer to its new location to begin drawing
                    lineRendParent.transform.position = new Vector3(gameObject.transform.position.x - GetComponent<Renderer>().bounds.extents.x,
                        gameObject.transform.position.y - GetComponent<Renderer>().bounds.extents.y, -10);
                 }
                break;
            case State.waiting:
                break;
            case State.pickLocation:
                // Randomly pick a set of objects to spawn as well as a location to spawn it at
                currentlySpawning = spawnObjects[Random.Range(0, spawnObjects.Length)];
                PickLocation();
                currentState = State.moving;
                spawnCount = 0;
                drawCount = 0;
                speed = 5.0f;
                break;
        }
	}
    // Calculate the speed that the spawner will need to move at in order to generate
    // an object within the spawn rate
    void CalcDrawingSpeed()
    {
        PolygonCollider2D coll = currentlySpawning.toSpawn.GetComponent<PolygonCollider2D>();
        if (coll == null)
        {
            speed = 1.0f;
            return;
        }
        points = coll.points;
        for (int i = 0; i < points.Length; i++)
        {
            points[i].x *= currentlySpawning.toSpawn.transform.localScale.x;
            points[i].y *= currentlySpawning.toSpawn.transform.localScale.y;
        }
        float dist = 0.0f;
        for (int i = 0; i < points.Length; i++)
        {
            dist += Vector3.Distance(points[i], points[(i + 1) % points.Length]);
        }
        speed = 1000*dist / currentlySpawning.spawnRate;
    }
    // Pick a random location to spawn the next set of objects at, and calculate
    // the distance from it to move to so that the spawner will appear to the 
    // top right of it
    void PickLocation()
    {
        spawnAt = GetRandomLocation();
        moveTo = spawnAt;
        offset = moveTo;
        moveTo.x += gameObject.GetComponent<Renderer>().bounds.extents.x;
        moveTo.y += gameObject.GetComponent<Renderer>().bounds.extents.y;
    }
    // Cleanup our line renderer here
    void OnDestroy()
    {
        Destroy(lineRendParent);
    }
}
