using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour
{
    public GameObject toChase;
    public int chaseWait = 2000;
    public float speed = 5;
    bool charging = false;
    int timeStart = 0;
    Vector3 velocity;
    // Use this for initialization
    void Start()
    {
        if (toChase == null)
            toChase = GameObject.Find("Player");
        timeStart = (int)Time.time * 1000;
    }
    // Update is called once per frame
    void Update()
    {
        if (charging)
        {
            transform.position += velocity * Time.deltaTime;
            return;
        }
        if (toChase == null)
            return;
        Transform faceTo = toChase.transform;
        if (faceTo.position != transform.position)
        {
            Vector3 lookPos = faceTo.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if ((Time.time * 1000) >= timeStart + chaseWait)
        {
            charging = true;
            velocity = transform.right * speed;
        }

    }
}
