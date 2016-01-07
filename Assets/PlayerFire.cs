using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the firing of bullets by the player as well as the players
/// reserves of each color
/// </summary>
public class PlayerFire : MonoBehaviour
{
    public long fireRate = 100;
    public int damage = 1;
    public float speed = 1;
    public GameObject bullet;
    public GameObject[] guns;
    long lastFired = 0;
    int[] energy;
    public int startEnergy = 128;
    public void Start()
    {
        energy = new int[guns.Length];
        for (int i = 0; i < energy.Length; i++)
            energy[i] = startEnergy;
    }
    /// <summary>
    /// Fire a bullet object in a particular direction
    /// </summary>
    /// <param name="direction">The direction to fire the bullet in</param>
    public void fire(Vector3 direction)
    {
        if (Input.GetButton("Absorb"))
        {
            return;
        }
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
    /// <summary>
    /// Add Red, Green, and/or Blue energy to the player
    /// </summary>
    /// <param name="input">The array of energy to add</param>
    public void addEnergy(int[] input)
    {
        for (int i = 0; i < input.Length; i++)
            energy[i] += input[i];
    }
    
    /// <summary>
    /// Get the energy array of the player
    /// </summary>
    /// <returns>The array of energy in the order Red, Green, Blue</returns>
    public int[] getEnergy()
    {
        return energy;
    }
}
