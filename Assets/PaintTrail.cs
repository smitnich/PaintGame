using UnityEngine;
using System.Collections;

public class PaintTrail : MonoBehaviour {
    public Color color = Color.red;
    GameObject floor;
    BlitColors script;
    Vector3 lastPosition;
    int size;
	// Use this for initialization
	void Start () {
        Vector3 extents = GetComponent<Renderer>().bounds.extents;
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<BlitColors>();
        size = script.determineSize(extents.x);
        lastPosition = transform.position;
	}
	
	void FixedUpdate () {
        script.setColor(transform.position, lastPosition, color, size);
        lastPosition = transform.position;
	}
}