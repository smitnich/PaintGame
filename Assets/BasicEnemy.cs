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
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
    public void damage(int damage)
    {
        health -= damage;
        if (health <= 0)
            die();
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
    private void die()
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
