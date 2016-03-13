using UnityEngine;
using System.Collections;

public class ExplodeOnDeath : MonoBehaviour {

    public float radius = 10;
    public bool isPlayer = true;
    public GameObject explosion;

	void OnDestroy()
    {
        GameObject obj = Instantiate(explosion);
        obj.transform.position = gameObject.transform.position;
        obj.GetComponent<DoExplosion>().StartExplosion(radius, isPlayer);
    }
}
