using UnityEngine;
using System.Collections;

/// <summary>
/// Basic script which handles enemies colliding with the player
/// and taking damage
/// </summary>
public class BasicEnemy : MonoBehaviour
{
    public int health = 1;
    public int speed = 5;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void damage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove playerScript = collision.gameObject.GetComponent<PlayerMove>();
        if (playerScript != null)
        {
            GameObject.Destroy(collision.gameObject);
            GameObject.Destroy(gameObject);
        }
    }
}
