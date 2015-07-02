using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
    public long fireRate = 100;
    public int damage = 1;
    public float speed = 1;
    public GameObject bullet;
    public GameObject[] guns;
    long lastFired = 0;
    int[] energy;
    public int startEnergy = 255;
    public void Start()
    {
        energy = new int[guns.Length];
        for (int i = 0; i < energy.Length; i++)
            energy[i] = startEnergy;
    }
    public void fire(Vector3 direction)
    {
        long now = (long)(Time.time * 1000);
        if (now > (lastFired + fireRate))
        {
            lastFired = now;
            for (int i = 0; i < guns.Length; i++ )
            {
                GameObject gun = guns[i];
                if (energy[i]-- <= 0)
                    continue;
                GameObject newBullet = Instantiate(bullet);
                newBullet.GetComponent<SetColor>().color = gun.GetComponent<SetColor>().color;
                newBullet.GetComponent<Rigidbody2D>().velocity = (direction * speed);
                newBullet.GetComponent<Rigidbody2D>().position = gun.transform.position;
                BulletCollision bulletScript = newBullet.GetComponent<BulletCollision>();
                bulletScript.damage = damage;
                bulletScript.isPlayerBullet = true;
            }
        }
    }
}
