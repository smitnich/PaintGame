using UnityEngine;
using System.Collections;

public class FireBullet : MonoBehaviour {
    public int speed = 5;
    public int damage = 1;
    public int fireDelay = 500;
    public GameObject bullet;
    long lastFired = 0;
    // Use this for initialization
    void Start() {
        lastFired = (long)(Time.time * 1000);
	}
	
	// Update is called once per frame
	void Update () {
	    long now = (long) (Time.time * 1000);
        if (now > lastFired + fireDelay)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.GetComponent<SetColor>().color = GetComponent<SetColor>().color;
            newBullet.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
            newBullet.transform.position = transform.position;
            newBullet.GetComponent<BulletCollision>().damage = damage;
            lastFired = now;
        }
	}
}
