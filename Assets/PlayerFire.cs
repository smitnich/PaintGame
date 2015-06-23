using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour {
    public long fireRate = 100;
    public int damage = 1;
    public float speed = 1;
    public GameObject bullet;
    long lastFired = 0;
    public void fire(Vector3 direction)
    {
        long now = (long) (Time.time * 1000);
        if (now > (lastFired + fireRate))
        {
            lastFired = now;
            GameObject newBullet = Instantiate(bullet);
            newBullet.GetComponent<SetColor>().color = GetComponent<SetColor>().color;
            newBullet.GetComponent<Rigidbody2D>().velocity = (direction * speed);
            newBullet.GetComponent<Rigidbody2D>().position = transform.position;
            BulletCollision bulletScript = newBullet.GetComponent<BulletCollision>();
            bulletScript.damage = damage;
            bulletScript.isPlayerBullet = true;
        }
    }
}
