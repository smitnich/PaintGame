using UnityEngine;
using System.Collections;

public class PaintTrail : MonoBehaviour {
    public Color color = Color.red;
    GameObject floor;
    BlitColors script;
    public int size = 10;
	// Use this for initialization
	void Start () {
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<BlitColors>();
	}
	
	void FixedUpdate () {
        script.setColor(transform.position.x, transform.position.y, color, size);
	}
}
