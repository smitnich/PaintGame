using UnityEngine;
using System.Collections;

/// <summary>
/// Handles controller input for the player; Movement, firing bullets
/// in a direction, and swapping colors
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public int moveSpeed = 1;
    public float deadzone = 0.2f;
    public float fireDeadzone = 0.7f;
    Color[] colors = { Color.red, Color.blue, Color.green };
    PlayerFire pf;
    SetColor SetColorScript;
    int colorIndex = 0;
    public GameObject objectWithin;
    // Use this for initialization
    void Start()
    {
        pf = (PlayerFire)GetComponent<PlayerFire>();
        SetColorScript = (SetColor)GetComponent<SetColor>();
        SetColorScript.ChangeColor(colors[0]);
    }

    /// <summary>
    /// Handle moving the player and checking if the player wants to change color
    /// </summary>
    void FixedUpdate()
    {
        if (Input.GetButton("Absorb"))
        {
            return;
        }
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector2 temp = new Vector2(horiz, vert);
        Vector3 objSize = objectWithin.GetComponent<Renderer>().bounds.size;
        transform.position += new Vector3(horiz, vert, 0) * Time.deltaTime * moveSpeed;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(horiz, vert) * -Mathf.Rad2Deg);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, objectWithin.transform.position.x - objSize.x / 2, objectWithin.transform.position.x + objSize.x / 2),
                                Mathf.Clamp(transform.position.y, objectWithin.transform.position.y - objSize.y / 2, objectWithin.transform.position.y + objSize.y / 2),
                                transform.position.z);
        horiz = Input.GetAxis("HorizontalFace");
        vert = Input.GetAxis("VerticalFace");
        temp = new Vector2(horiz, vert);
        if (horiz != 0.0f && vert != 0.0f)
        {
            Vector3 direction = new Vector3(horiz, vert, 0.0f).normalized;
            if (temp.magnitude > deadzone)
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(horiz, vert) * -Mathf.Rad2Deg);
            if (temp.magnitude > fireDeadzone)
                pf.fire(direction);
        }
        if (Input.GetAxis("Missile") != 0.0f)
        {
            pf.fireMissile(transform.up);
        }
        if (Input.GetAxis("Bomb") != 0.0f)
        {
            GetComponent<Explode>().explode();
        }
        checkColorSwap();
    }
    /// <summary>
    /// Check if the player wants to change color based
    /// on the SwapColorDown or SwapColorUp buttons being pressed
    /// </summary>
    void checkColorSwap()
    {
        if (Input.GetButtonDown("SwapColorDown"))
        {
            colorIndex -= 1;
            if (colorIndex < 0)
                colorIndex += colors.Length;
            SetColorScript.ChangeColor(colors[colorIndex]);
        }
        if (Input.GetButtonDown("SwapColorUp"))
        {
            colorIndex += 1;
            if (colorIndex >= colors.Length)
                colorIndex -= colors.Length;
            SetColorScript.ChangeColor(colors[colorIndex]);
        }
    }
}
