﻿using UnityEngine;
using System.Collections;


/// <summary>
/// Set an object to have a texture of a particular color
/// </summary>
public class SetColor : MonoBehaviour {
    public Color color = Color.red;
    Renderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = color;
	}
    /// <summary>
    /// Change the objects color
    /// </summary>
    /// <param name="_color">The new color of the object</param>
    public void ChangeColor(Color _color)
    {
        this.color = _color;
        if (rend != null)
            rend.material.color = this.color;
    }
}
