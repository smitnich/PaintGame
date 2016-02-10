using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior for enemies which causes them to attempt to avoid the player
/// </summary>
public class avoidPlayer : MonoBehaviour
{
    public bool faceTowards = true;
    public GameObject player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    /// <summary>
    /// Rotate away from the player
    /// </summary>
    void Update()
    {
        if (player == null)
            return;
        Transform faceTo = player.transform;
        if (faceTo.position != transform.position)
        {
            Vector3 lookPos = faceTo.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            if (faceTowards == false)
            {
                angle += 180;
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
