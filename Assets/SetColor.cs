using UnityEngine;
using System.Collections;

public class SetColor : MonoBehaviour {
    public Color color = Color.red;
    Renderer renderer;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = color;
	}
    public void ChangeColor(Color _color)
    {
        this.color = _color;
        if (renderer != null)
            renderer.material.color = this.color;
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
