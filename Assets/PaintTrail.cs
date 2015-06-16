using UnityEngine;
using System.Collections;

public class PaintTrail : MonoBehaviour {
    public Color color = Color.red;
    GameObject floor;
    BlitColors script;
    public int size = 10;
    Vector3 lastPosition;
	// Use this for initialization
	void Start () {
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<BlitColors>();
        lastPosition = transform.position;
	}
	
	void FixedUpdate () {
        script.setColor(transform.position, lastPosition, color, size);
        lastPosition = transform.position;
	}
}