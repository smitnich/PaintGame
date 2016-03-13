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
    public AudioSource deathSound;
    public bool leavePaintOnDeath = true;
    public bool isBullet;
    FloorManager script;
    // Use this for initialization
    void Start()
    {
        GameObject floor = GameObject.Find("Floor");
        Vector3 extents = GetComponent<Renderer>().bounds.extents;
        script = floor.GetComponent<FloorManager>();
        script = floor.GetComponent<FloorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // A speed of -1 means to not modify speed in this script
        if (speed != -1)
            GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
    public void damage(int damage)
    {
        // A health value of -1 means that health should be ignored
        if (health == -1)
            return;
        health -= damage;
        if (health <= 0)
            die();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't check for collisions with other bullets
        if (isBullet)
            return;
        PlayerMove playerScript = collision.gameObject.GetComponent<PlayerMove>();
        if (playerScript != null)
        {
            GameObject.Destroy(collision.gameObject);
            GameObject.Destroy(gameObject);
        }
    }
    public void die()
    {
        if (deathSound != null)
            deathSound.Play();
        if (leavePaintOnDeath)
            leavePaint();
        Destroy(gameObject);
    }
    private void leavePaint()
    {
        // Compute the bounding box of the rotated object; this should be the maximum of the x and y extents
        Bounds b = GetComponent<Renderer>().bounds;
        float length = Mathf.Max(b.max.x - b.min.x, b.max.y - b.min.y) / 2;
        Vector2 start = new Vector2(b.center.x - length, b.center.y - length);
        Vector2 end = new Vector2(b.center.x + length, b.center.y + length);
        script.FillRaycast(start, end, gameObject);
        // Iterate across every pixel within this range, and check via raycast if the object's collider
        // is within that pixel
        // If so, color that pixel to the proper color
    }
}
