using UnityEngine;
using System.Collections;

public class AbsorbColor : MonoBehaviour {
    SetColor colorScript;
    PlayerFire fireScript;
    FloorManager floor;
    public int absorbStrength = 5;
    public int maxRadius = 1000;
    int currentRadius = 5;
    int startRadius = 10;
    int radiusStep = 1;
	// Use this for initialization
	void Start () {
        fireScript = GetComponent<PlayerFire>();
        GameObject tmpFloor = GameObject.Find("Floor");
        floor = tmpFloor.GetComponent<FloorManager>();
    }
	// Update is called once per frame
	void Update () {
        int[] result = {0,0,0};
        //blitScript.setColor(transform.position, transform.position, Color.white, 1, 0);
        if (Input.GetButton("Absorb"))
        {
            if (currentRadius > startRadius)
            {
                result = floor.absorbCircleRadius(transform.position, currentRadius, absorbStrength);
                float area = 2 * currentRadius * Mathf.PI;
                for (int i = 0; i < result.Length; i++)
                    result[i] = (int)(result[i] / (area));
                fireScript.addEnergy(result);
                if (currentRadius < maxRadius)
                    currentRadius += radiusStep;
            }
            else
            {
                result = floor.absorbCircle(transform.position, currentRadius, absorbStrength);
                float area = currentRadius * currentRadius * Mathf.PI;
                for (int i = 0; i < result.Length; i++)
                    result[i] = (int)(result[i] / (area));
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
