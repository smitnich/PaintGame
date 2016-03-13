using UnityEngine;
using System.Collections;

/// <summary>
/// Handles enemies firing bullets at a predetermined rate
/// </summary>
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
	/// <summary>
    /// Check if need to fire a bullet due to the fireDelay being finished
    /// </summary>
    void Update () {
	    long now = (long) (Time.time * 1000);
        if (now > lastFired + fireDelay)
        {
            GameObject newBullet = (GameObject) Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = Vector3.up * speed;
            newBullet.GetComponent<SetColor>().color = GetComponent<SetColor>().color;
            newBullet.GetComponent<BulletCollision>().damage = damage;
            lastFired = now;
        }
	}
}
