using UnityEngine;

/// <summary>
/// Used to cause this object to follow a particular object
/// </summary>
public class Chase : MonoBehaviour {
    public float speed = 5;
    public GameObject toFollow;

	// Use this for initialization
	void Start () {
        if (toFollow == null)
            toFollow = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	/// <summary>
    /// Follow the object!
    /// </summary>
    void Update () {
        if (toFollow == null)
            return;
        Transform faceTo = toFollow.transform;
        if (faceTo.position != transform.position)
        {
            Vector3 lookPos = faceTo.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        transform.position += transform.right * speed * Time.deltaTime;
	}
}
