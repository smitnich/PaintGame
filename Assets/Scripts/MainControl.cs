using UnityEngine;
using System.Collections;

public class MainControl : MonoBehaviour {
    // Controls input that doesn't directly affect the player
    // such as exiting the game
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButton("Quit"))
        {
            Application.Quit();
        }
	}
}
