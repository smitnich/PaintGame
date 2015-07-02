using UnityEngine;
using System.Collections;

public class LimitedLifetime : MonoBehaviour {
    public int lifeTime = 2000;
    int dieTime;
	// Use this for initialization
	void Start () {
        dieTime = (int) (Time.time * 1000) + lifeTime;	    
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.time * 1000) >= dieTime)
            GameObject.Destroy(gameObject);
	}
}
