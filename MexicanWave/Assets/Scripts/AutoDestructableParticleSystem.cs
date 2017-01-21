using UnityEngine;
using System.Collections;

public class AutoDestructableParticleSystem : MonoBehaviour
{

	private ParticleSystem ps;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if (!ps.IsAlive() || !ps.isPlaying)
		{
			Destroy(gameObject);
		}
	}
}
