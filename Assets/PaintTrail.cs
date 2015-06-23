using UnityEngine;
using System.Collections;

public class PaintTrail : MonoBehaviour
{
    public int absorbStrength = 0;
    Color color;
    GameObject floor;
    BlitColors script;
    Vector3 lastPosition;
    bool firstUpdate = true;
    int size;
    // Use this for initialization
    void Start()
    {
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<BlitColors>();
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
                script.setColor(transform.position, lastPosition, color, size, absorbStrength);
        }
        lastPosition = transform.position;
    }
}