using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {
    public int health = 1;
    public int speed = 5;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void damage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }
}
