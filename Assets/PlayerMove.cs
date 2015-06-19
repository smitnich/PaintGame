using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public int moveSpeed = 1;
    public float deadzone = 0.4f;
    Color[] colors = { Color.magenta, Color.cyan, Color.yellow };
    PlayerFire pf;
    SetColor setColorScript;
    int colorIndex = 0;
    public GameObject objectWithin;
	// Use this for initialization
	void Start () {
        pf = (PlayerFire)GetComponent<PlayerFire>();
        setColorScript = (SetColor)GetComponent<SetColor>();
        setColorScript.ChangeColor(colors[0]);
	}
	
	void FixedUpdate () {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector2 temp = new Vector2(horiz, vert);
        Vector3 objSize = objectWithin.GetComponent<Renderer>().bounds.size;
        transform.position += new Vector3(horiz,vert,0)*Time.deltaTime*moveSpeed;
        transform.LookAt(transform.position + new Vector3(horiz, vert, 0.0f).normalized, -Vector3.forward);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, objectWithin.transform.position.x - objSize.x / 2, objectWithin.transform.position.x + objSize.x / 2),
                                Mathf.Clamp(transform.position.y, objectWithin.transform.position.y - objSize.y / 2, objectWithin.transform.position.y + objSize.y / 2),
                                transform.position.z);
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
        checkColorSwap();
    }
    void checkColorSwap()
    {
        if (Input.GetButtonDown("SwapColorDown"))
        {
            colorIndex -= 1;
            if (colorIndex < 0)
                colorIndex += colors.Length;
            setColorScript.ChangeColor(colors[colorIndex]);
        }
        if (Input.GetButtonDown("SwapColorUp"))
        {
            colorIndex += 1;
            if (colorIndex >= colors.Length)
                colorIndex -= colors.Length;
            setColorScript.ChangeColor(colors[colorIndex]);
        }
    }
}
