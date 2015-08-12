using UnityEngine;
using System.Collections;

public class AmmoBar : MonoBehaviour {

     float[] barDisplay; //current progress
     public GameObject[] sliders;
     GameObject player;

     void Start()
     {
         player = GameObject.Find("Player");
         barDisplay = new float[sliders.Length];
         initTextures();
     }
     public void initTextures()
     {
     }
     void OnGUI() {
         for (int i = 0; i < sliders.Length; i++) {
            sliders[i].GetComponent<UnityEngine.UI.Slider>().value = barDisplay[i];
         }
     }
     
     void Update() {
         //for this example, the bar display is linked to the current time,
         //however you would set this value based on your desired display
         //eg, the loading progress, the player's health, or whatever.
         for (int i = 0; i < sliders.Length; i++)
         {
             int[] energy = player.GetComponent<PlayerFire>().getEnergy();
             barDisplay[i] = energy[i];
         }
 //        barDisplay = MyControlScript.staticHealth;
     }
 }
