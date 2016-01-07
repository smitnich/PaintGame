using UnityEngine;
using System.Collections;


/// <summary>
/// Set an object to have a texture of a particular color
/// </summary>
public class SetColor : MonoBehaviour {
    public Color color = Color.red;
    Renderer renderer;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = color;
	}
    /// <summary>
    /// Change the objects color
    /// </summary>
    /// <param name="_color">The new color of the object</param>
    public void ChangeColor(Color _color)
    {
        this.color = _color;
        if (renderer != null)
            renderer.material.color = this.color;
    }
}
