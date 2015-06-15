using UnityEngine;
using System.Collections;

public class PixelTest : MonoBehaviour {
    Texture texture;
	// Use this for initialization
	void Start () {
        int size = 1024;
        Texture2D temp = new Texture2D(size, size);
        UnityEngine.Color[] colors = new UnityEngine.Color[size*size];
        for (int i = 0; i < size*size; i++)
            colors[i] = Color.white;
        temp.SetPixels(colors);
        GetComponent<Renderer>().material.mainTexture = temp;
        temp.Apply();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
