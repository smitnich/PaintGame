using UnityEngine;

/// <summary>
/// Add to an object to make it die after a
/// predetermined length of time
/// </summary>
public class LimitedLifetime : MonoBehaviour {
    public int lifeTime = 2000;
    int dieTime;
	// Use this for initialization
	void Start () {
        dieTime = (int) (Time.time * 1000) + lifeTime;	    
	}
	
	// Update is called once per frame
	/// <summary>
    /// Checks if death has come for this object
    /// </summary>
    void Update () {
        if ((Time.time * 1000) >= dieTime)
        {
            BasicEnemy script = GetComponent<BasicEnemy>();
            if (script != null)
                script.die();
            else
                GameObject.Destroy(gameObject);
        }
	}
}
