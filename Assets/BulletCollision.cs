using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour {
    public int damage = 0;
    public bool isPlayerBullet = false;
    public void Start()
    {

    }
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }
}
