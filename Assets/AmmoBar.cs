using UnityEngine;
using System.Collections;

/// <summary>
/// Displays the player's energy of a particular type
/// </summary>
public class AmmoBar : MonoBehaviour {

     float[] barDisplay; //current progress
     public GameObject[] sliders;
     GameObject player;

     void Start()
     {
         player = GameObject.Find("Player");
         barDisplay = new float[sliders.Length];
     }
     /// <summary>
     /// Update the GUI with the new energy value
     /// </summary>
     void OnGUI() {
         for (int i = 0; i < sliders.Length; i++) {
            sliders[i].GetComponent<UnityEngine.UI.Slider>().value = barDisplay[i];
         }
     }
     
     /// <summary>
     /// Update the cached value of the player's energy
     /// </summary>
     void Update() {
        if (player == null)
            return;
         for (int i = 0; i < sliders.Length; i++)
         {
             int[] energy = player.GetComponent<PlayerFire>().getEnergy();
             barDisplay[i] = energy[i];
         }
     }
 }
