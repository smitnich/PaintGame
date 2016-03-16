using UnityEngine;
using System.Collections;

public class DoExplosion : MonoBehaviour {

    private float finalRadius;
    public float expandRate = 1.0f;
    private float size = 1.0f;
    private bool isPlayer;
    public int damage = 1000;

    public void StartExplosion(float radius, bool _isPlayer)
    {
        finalRadius = radius;
        isPlayer = _isPlayer;
	}
	
	// Update is called once per frame
	void Update () {
        size += expandRate * Time.deltaTime;
        transform.localScale = new Vector3(size, size, size);
        if (size >= finalRadius)
        {
            GameObject.Destroy(gameObject);
            GameObject floor = GameObject.Find("Floor");
            if (floor == null)
                return;
            FloorManager script = floor.GetComponent<FloorManager>();
            if (script == null)
                return;
            Vector2 pixels = script.GameCoordsToPixel(transform.position.x, transform.position.y);
            Color color = GetComponent<SetColor>().color;
            script.SetPixelCircle((int) pixels.x, (int) pixels.y, script.determineSize(GetComponent<Renderer>().bounds.extents.x/2), color);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        BasicEnemy be = coll.gameObject.GetComponent<BasicEnemy>();
        if (be != null)
        {
            be.damage(damage);
        }
    }
}
