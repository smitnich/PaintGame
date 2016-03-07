using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {

    public float radius = 10;
    public GameObject explosion = null;
    public bool isPlayer = true;

    public long bombDelay = 5000;
    private long lastExplode = System.Int64.MinValue;

	public void explode() {
        long now = (long) Time.time * 1000;
        // If it hasn't been the length of bombDelay since we last set a bomb,
        // don't set one now
        if (lastExplode + bombDelay > now)
            return;
        lastExplode = now;
        GameObject obj = Instantiate(explosion);
        obj.transform.position = gameObject.transform.position;
        obj.GetComponent<DoExplosion>().StartExplosion(radius, isPlayer);
	}
}
