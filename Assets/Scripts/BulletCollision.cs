using UnityEngine;

/// <summary>
/// Handles checking for collisions with bullets,
/// both for the player and enemies
/// </summary>
public class BulletCollision : MonoBehaviour {
    public int damage = 0;
    public bool isPlayerBullet = false;
    public void Start()
    {

    }
    /// <summary>
    /// Damage either the enemy or the player
    /// </summary>
    /// <param name="collision">The collision that led to this call</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerBullet)
        {
            BasicEnemy otherScript = collision.gameObject.GetComponent<BasicEnemy>();
            if (otherScript != null)
            {
                otherScript.damage(damage);
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            PlayerMove playerScript = collision.gameObject.GetComponent<PlayerMove>();
            if (playerScript != null)
            {
                GameObject.Destroy(collision.gameObject);
                GameObject.Destroy(gameObject);
            }
        }
    }

}
