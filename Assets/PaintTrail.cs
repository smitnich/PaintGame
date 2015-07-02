using UnityEngine;
using System.Collections;

public class PaintTrail : MonoBehaviour
{
    Color color;
    GameObject floor;
    FloorManager script;
    Vector3 lastPosition;
    bool firstUpdate = true;
    int size;
    // Use this for initialization
    void Start()
    {
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<FloorManager>();
    }

    void FixedUpdate()
    {
        color = GetComponent<SetColor>().color;
        if (firstUpdate)
        {
            Vector3 extents = GetComponent<Renderer>().bounds.extents;
            size = script.determineSize(extents.x);
            lastPosition = transform.position;
            firstUpdate = false;
            return;
        }
        if (script != null)
        {
            script.setColor(transform.position, lastPosition, color, size);
        }
        lastPosition = transform.position;
    }
}