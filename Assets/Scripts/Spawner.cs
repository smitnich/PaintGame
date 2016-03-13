using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn objects over time, either randomly or at a set rate
/// </summary>
public class Spawner : MonoBehaviour {
    public GameObject[] toSpawn;
    public bool isRandom = true;
    public int count = -1;
    public int spawnDelay = 500;
    int spawned = 0;
    long lastSpawn = 0;
    float depth = 0;
    float speed = 25;
    GameObject player;
    public GameObject objectWithin;
	// Use this for initialization
	void Start () {
        if (count == -1)
        {
            count = toSpawn.Length;
        }
        else
        {
            GameObject[] tmpArray = new GameObject[count];
            for (int i = 0; i < count; i++)
                tmpArray[i] = toSpawn[i % toSpawn.Length];
            toSpawn = tmpArray;
        }
        lastSpawn = (long) Time.time * 1000;
        player = GameObject.Find("Player");
        depth = player.transform.position.z;
	}
	
	// Update is called once per frame
	/// <summary>
    /// Check if any objects should be made, and do so if necessary
    /// </summary>
    void Update () {
        long time = (long) (Time.time * 1000);
        if (time >= lastSpawn + spawnDelay)
        {
            lastSpawn = (long) (Time.time * 1000);
            spawnObject();
            if (spawned >= count)
            {
                GameObject.Destroy(gameObject);
                return;
            }
        }
        if (player == null)
            return;
        Transform faceTo = player.transform;
        Vector3 objSize = objectWithin.GetComponent<Renderer>().bounds.size;
        transform.position += (transform.position - faceTo.transform.position).normalized * speed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, objectWithin.transform.position.x - objSize.x / 2, objectWithin.transform.position.x + objSize.x / 2),
                                Mathf.Clamp(transform.position.y, objectWithin.transform.position.y - objSize.y / 2, objectWithin.transform.position.y + objSize.y / 2),
                                transform.position.z);
    }
    /// <summary>
    /// Create an object within the world
    /// </summary>
    void spawnObject()
    {
        GameObject obj = Instantiate(toSpawn[spawned++]);
        obj.transform.position = new Vector3(transform.position.x,transform.position.y,depth);
    }
}
