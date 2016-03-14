using UnityEngine;
using System.Collections;

public class ExplodeOnDeath : MonoBehaviour {

    public float radius = 10;
    public bool isPlayer = true;
    public GameObject explosion;

    private bool exitRequested = false;

	public void OnDestroy()
    {
        if (exitRequested)
            return;
        GameObject obj = Instantiate(explosion);
        obj.GetComponent<SetColor>().color = GetComponent<SetColor>().color;
        obj.transform.position = gameObject.transform.position;
        obj.GetComponent<DoExplosion>().StartExplosion(radius, isPlayer);
    }
    public void OnApplicationQuit()
    {
        exitRequested = true;
    }
}
