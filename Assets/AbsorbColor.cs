using UnityEngine;
using System.Collections;

/// <summary>
/// Check if the player has pressed the Absorb button,
/// and if so change the color of the floor and increase
/// the player's energy.
/// The radius to absorb within increases over time up to a
/// maximum limit
/// </summary>
public class AbsorbColor : MonoBehaviour {
    PlayerFire fireScript;
    FloorManager floor;
    public int absorbStrength = 5;
    public int maxRadius = 1000;
    int currentRadius = 5;
    int startRadius = 10;
    int radiusStep = 1;
    int[] acummulatedEnergy = { 0, 0, 0 };
	// Use this for initialization
	void Start () {
        fireScript = GetComponent<PlayerFire>();
        GameObject tmpFloor = GameObject.Find("Floor");
        floor = tmpFloor.GetComponent<FloorManager>();
    }
	// Update is called once per frame
	/// <summary>
    /// Check if the player wishes to absorb color from the floor
    /// If not, reset the radius to the smallest size
    /// </summary>
    void Update () {
        int[] result = {0,0,0};
        if (Input.GetButton("Absorb"))
        {
            if (currentRadius > startRadius)
            {
                result = floor.absorbCirclePerimeter(transform.position, currentRadius, absorbStrength);
                //float area = 2 * currentRadius * Mathf.PI;
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] += acummulatedEnergy[i];
                    acummulatedEnergy[i] = result[i] % (255 * 100);
                    result[i] = (int)(result[i] / (255 * 100));
                }
                fireScript.addEnergy(result);
                if (currentRadius < maxRadius)
                    currentRadius += radiusStep;
            }
            else
            {
                result = floor.absorbCircle(transform.position, currentRadius, absorbStrength);
                //float area = currentRadius * currentRadius * Mathf.PI;
                for (int i = 0; i < result.Length; i++)
                    result[i] = (int)(result[i] / (255*100));
                fireScript.addEnergy(result);
                currentRadius += radiusStep;
            }
        }
        else
        {
            currentRadius = startRadius;
        }
    }
}
