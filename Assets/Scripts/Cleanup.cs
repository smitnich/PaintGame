using UnityEngine;
using System.Collections;

/// <summary>
/// Delete any objects that have moved outside of the level
/// </summary>
public class Cleanup : MonoBehaviour {
    public GameObject objectWithin;
    public int range = 2;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 size = objectWithin.GetComponent<Renderer>().bounds.size*2;
        if (this.transform.position.x < (objectWithin.transform.position.x-size.x/2)-range
         || this.transform.position.y < (objectWithin.transform.position.y-size.y/2)-range
         || this.transform.position.x > (objectWithin.transform.position.x+size.x/2)+range
         || this.transform.position.y > (objectWithin.transform.position.y+size.y/2)+range)
        {
            Destroy(gameObject);
        }
	}
}
