using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {

    // Minimum amount of time to move in the same direction
    public int minMoveTime = 1000;
    // Maximum amount of time to move in the same direction
    public int maxMoveTime = 1000;
    private int moveTime = 500;

    public GameObject stayWithin;
    private Vector3 boundsWithin;
    private Vector3 speed;
    private int scalarSpeed;

    private long lastMoveTime = -1;
    private float minX, maxX, minY, maxY;

	// Use this for initialization
	void Start () {
	    if (stayWithin == null)
        {
            stayWithin = GameObject.Find("Floor");
        }
        boundsWithin = stayWithin.GetComponent<Renderer>().bounds.size;
        minX = (stayWithin.transform.position.x - boundsWithin.x / 2);
        maxX = (stayWithin.transform.position.x + boundsWithin.x / 2);
        minY = (stayWithin.transform.position.y - boundsWithin.y / 2);
        maxY = (stayWithin.transform.position.y + boundsWithin.y / 2);
        moveTime = Random.Range((int) minMoveTime, (int) maxMoveTime);
        scalarSpeed = GetComponent<BasicEnemy>().speed;
        GetComponent<BasicEnemy>().speed = -1;
    }
	
	// Update is called once per frame
	void Update () {
        if (lastMoveTime == -1 || Time.time * 1000 >= lastMoveTime + moveTime)
        {
            int angle = Random.Range(0, 360);
            Vector2 vec = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle)).normalized;
            speed = vec*scalarSpeed;
            moveTime = Random.Range(minMoveTime, maxMoveTime);
            lastMoveTime = (long) Time.time * 1000;
        }
        else
        {
            transform.position += speed*Time.deltaTime;
        }
        if (transform.position.x <= minX)
        {
            speed.x = Mathf.Abs(speed.x);
        }
        else if (transform.position.x >= maxX)
        {
            speed.x = -Mathf.Abs(speed.x);
        }
        if (transform.position.y <= minY)
        {
            speed.y = Mathf.Abs(speed.y);
        }
        else if (transform.position.y >= maxY)
        {
            speed.y = -Mathf.Abs(speed.y);
        }
    }
}
