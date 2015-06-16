using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public int moveSpeed = 1;
    public float deadzone = 0.4f;
    PlayerFire pf;
	// Use this for initialization
	void Start () {
        pf = (PlayerFire)GetComponent<PlayerFire>();
	}
	
	void FixedUpdate () {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        //Debug.Log(string.Format("Horiz = {0}|Vert = {1}", horiz, vert));
        Vector2 temp = new Vector2(horiz, vert);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        transform.position += new Vector3(horiz,vert,0)*Time.deltaTime*moveSpeed;
        transform.LookAt(transform.position + new Vector3(horiz, vert, 0.0f).normalized, -Vector3.forward);
        horiz = Input.GetAxis("HorizontalFace");
        vert  = Input.GetAxis("VerticalFace");
        temp = new Vector2(horiz, vert);
        if (horiz != 0.0f && vert != 0.0f)
        {
            Vector3 direction = new Vector3(horiz, vert, 0.0f).normalized;
            transform.LookAt(transform.position + direction, -Vector3.forward);
            if (temp.magnitude > 0.5)
                pf.fire(direction);
        }
    }
}
