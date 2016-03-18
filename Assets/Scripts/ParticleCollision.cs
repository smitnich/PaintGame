using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour {

    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1024];
    FloorManager manager;
    ParticleSystem parent;

    float deathDepth = 0.0f;

    void Start()
    {
        manager = GameObject.Find("Floor").GetComponent<FloorManager>();
        deathDepth = manager.transform.position.z;
        parent = GetComponentInParent<ParticleSystem>();
    }

    public void OnParticleCollision(GameObject obj)
    {
        int num = parent.GetComponent<ParticleSystem>().GetParticles(particles);
        for (int i = 0; i < num; i++)
        {
            ParticleSystem.Particle part = particles[i];
            Vector3 pos = part.position;
            //if (pos.z > deathDepth)
            {
                Vector2 coord = manager.GameCoordsToPixel(pos.x, pos.y);
                manager.SetPixelCircle((int) coord.x, (int) coord.y, 5, part.GetCurrentColor(parent));
                part.lifetime = -1;
            }
            particles[i] = part;
        }
        parent.GetComponent<ParticleSystem>().SetParticles(particles, num);
    }
    public void Update()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
