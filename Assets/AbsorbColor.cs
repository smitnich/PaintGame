using UnityEngine;
using System.Collections;

public class AbsorbColor : MonoBehaviour {
    int red, green, blue;
    SetColor colorScript;
    BlitColors blitScript;
	// Use this for initialization
	void Start () {
        colorScript = GetComponent<SetColor>();
        Color tmpColor = colorScript.color;
        red = (int) (tmpColor.r*255);
        green = (int) (tmpColor.g * 255);
        blue = (int) (tmpColor.b * 255);
	}
    Color determineColor()
    {
        return new Color((float) (red/255.0), (float) (green/255.0), (float) (blue/255.0));
    }
	// Update is called once per frame
	void Update () {
        //blitScript.setColor(transform.position, transform.position, Color.white, 1, 0);
        colorScript.ChangeColor(determineColor());
	}
}
