using UnityEngine;
using System.Collections;

public class ParticlesOnDeath : MonoBehaviour {

    public ParticleSystem system;

    public void Start()
    {
        system.Stop();
        system.Clear();
    }

	public void OnDestroy()
    {
        ParticleSystem newSystem = (ParticleSystem) Instantiate(system, transform.position, transform.rotation);
        newSystem.Play();
    }
	
}
