using UnityEngine;
using System.Collections;

public class SpawnDisabled : MonoBehaviour {

    SetColor colorScript = null;
    // We don't want a disabled object to move before
    // its timer has run out; since there are many different
    // ways it could move, we simply lock its position to its
    // initial location
    Vector3 lockPos;
    public bool disabled = true;
    public int timer = 1000;
    int freeTime = 0;

	// Use this for initialization
	void Start () {
        colorScript = (SetColor)GetComponent<SetColor>();
        colorScript.ChangeColor(Color.black);
        lockPos = transform.position;
        freeTime = timer + ((int) Time.time * 1000);
    }
	
    void LateUpdate() {
        if (!disabled)
            return;
        if (Time.time * 1000 >= freeTime)
        {
            disabled = false;
        }
        if (disabled)
        {
            transform.position = lockPos;
        }
    }
}
