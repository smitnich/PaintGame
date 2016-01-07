using UnityEngine;
using System.Collections;

/// <summary>
/// Leave behind a circle of color when this object is destroyed
/// </summary>
public class ColorOnDeath : MonoBehaviour {
    FloorManager script;
    int size;
    Color color;
	// Use this for initialization
	void Start () {
        GameObject floor = GameObject.Find("Floor");
        Vector3 extents = GetComponent<Renderer>().bounds.extents;
        script = floor.GetComponent<FloorManager>();
        color = GetComponent<SetColor>().color;
        size = script.determineSize(extents.x);
    }
	
    /// <summary>
    /// Leave a circle of color based on our size, color, and location
    /// </summary>
    void OnDestroy()
    {
        if (script != null)
            script.setColor(transform.position, transform.position, color, size);
    }
}
