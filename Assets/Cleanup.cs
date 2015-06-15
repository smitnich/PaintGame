using UnityEngine;
using System.Collections;

public class Cleanup : MonoBehaviour {
    public GameObject objectWithin;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 size = objectWithin.GetComponent<Renderer>().bounds.size;
        if (this.transform.position.x < (objectWithin.transform.position.x-size.x/2)
         || this.transform.position.y < (objectWithin.transform.position.y-size.y/2)
         || this.transform.position.x > (objectWithin.transform.position.x+size.x/2)
         || this.transform.position.y > (objectWithin.transform.position.y+size.y/2))
        {
            Destroy(gameObject);
        }
	}
}
