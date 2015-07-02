using UnityEngine;
using System.Collections;

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
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if (script != null)
            script.setColor(transform.position, transform.position, color, size);
    }
}
