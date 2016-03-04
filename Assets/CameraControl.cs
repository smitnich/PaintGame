using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public GameObject toFollow;
    public GameObject boundary;
    Bounds floorBounds;
    float xMin, xMax, yMin, yMax;

	// Use this for initialization
	void Start () {
        floorBounds = boundary.GetComponent<Renderer>().bounds;
        Vector3 max = floorBounds.max;
        Vector3 min = floorBounds.min;
        Camera cam = GetComponentInParent<Camera>();
        float height = cam.orthographicSize;
        float width = cam.orthographicSize * cam.aspect;
        xMin = min.x + width;
        xMax = max.x - width;
        yMin = min.y + height;
        yMax = max.y - height;
    }
	
	// Update is called once per frame
	void Update () {
        if (toFollow == null)
            return;
        float xPos = Mathf.Clamp(toFollow.transform.position.x, xMin, xMax);
        float yPos = Mathf.Clamp(toFollow.transform.position.y, yMin, yMax);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
