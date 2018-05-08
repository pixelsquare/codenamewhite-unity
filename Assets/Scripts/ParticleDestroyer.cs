using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour 
{
	private ParticleSystem m_particle	= null;

	private void Start()
	{
		m_particle = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if(m_particle != null)
		{
			if(!m_particle.IsAlive())
			{
				DestroyImmediate(gameObject);
			}
		}
	}
}
