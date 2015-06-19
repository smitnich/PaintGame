using UnityEngine;
using System.Collections;

public class PaintTrail : MonoBehaviour {
    Color color;
    GameObject floor;
    BlitColors script;
    Vector3 lastPosition;
    bool firstUpdate = true;
    int size;
	// Use this for initialization
	void Start () {
        color = GetComponent<SetColor>().color;
        Vector3 extents = GetComponent<Renderer>().bounds.extents;
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<BlitColors>();
        size = script.determineSize(extents.x);
	}
	
	void FixedUpdate () {
        if (firstUpdate)
        {
            lastPosition = transform.position;
            firstUpdate = false;
            return;
        }
        if (script != null)
            script.setColor(transform.position, lastPosition, color, size);
        lastPosition = transform.position;
	}
}