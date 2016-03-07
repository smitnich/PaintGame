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
            GameObject.Destroy(gameObject);
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
