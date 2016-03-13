using UnityEngine;
using System.Collections;

// Use this for initialization
/// <summary>
/// Add to an object to have it leave behind a trail of
/// a particular color
/// </summary>
public class PaintTrail : MonoBehaviour
{
    Color color;
    GameObject floor;
    FloorManager script;
    Vector3 lastPosition;
    bool firstUpdate = true;
    int size;

    void Start()
    {
        floor = GameObject.Find("Floor");
        script = floor.GetComponent<FloorManager>();
    }

    /// <summary>
    /// Lay down a trail between the previous position and
    /// the new position every frane
    /// </summary>
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
            script.SetColor(transform.position, lastPosition, color, size);
        }
        lastPosition = transform.position;
    }
}