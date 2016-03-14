using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    public GameObject[] spawnObjects = { null };
    public int spawnRate = 4000;
    public int spawnNumber = 10;
    public int spawnDelay = 100;
    public GameObject spawnWithin = null;
    float maxX = 0;
    float minX = 0;
    float maxY = 0;
    float minY = 0;
    int lastSpawnTime = 0;
    int spawnCount = 0;
    bool spawning = false;
    Vector3 spawnAt;
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
    }
	
    Vector3 getRandomLocation()
    {
        Vector3 location;
        Vector3 playerLocation = player.transform.position;
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
    void spawnObject()
    {
        GameObject newObj = Instantiate(spawnObjects[0]);
        newObj.transform.position = spawnAt;
    }
    // Update is called once per frame
    void Update () {
        int curTime = (int)(Time.time * 1000);
        if (spawning)
        {
            if (curTime >= spawnDelay + lastSpawnTime)
            {
                spawnObject();
                spawnCount++;
                if (spawnCount >= spawnNumber)
                {
                    spawning = false;
                    spawnCount = 0;
                }
                lastSpawnTime = curTime;
            }
        }
        else if (curTime >= spawnRate + lastSpawnTime)
        {
            spawnAt = getRandomLocation();
            spawning = true;
        }
	}
}
