using UnityEngine;
using System.Collections;


public class InportImageFile : MonoBehaviour {
    Color[] image;
	// Use this for initialization
	void Start () {
        Texture2D tmp = (Texture2D) Resources.Load("testImage.png");
        image = tmp.GetPixels(0, 0, tmp.width, tmp.height);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
