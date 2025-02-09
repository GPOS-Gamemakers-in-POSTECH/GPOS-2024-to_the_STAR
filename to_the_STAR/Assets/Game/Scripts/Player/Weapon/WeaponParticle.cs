using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponParticle : MonoBehaviour
{
    public GameObject player;
    public GameObject TargetParticle;
    public ParticleSystem ParticleSys;

    private void Update()
    {
        ParticleSystem.ForceOverLifetimeModule forceModule = ParticleSys.forceOverLifetime;

        if (!ParticleSys.isPlaying)
        {
            if (player != null && TargetParticle != null)
            {
                TargetParticle.transform.position = player.transform.position;
                TargetParticle.transform.rotation = player.transform.rotation;
            }
        }
    }

    public void playParticle()
    {
        ParticleSys.Play();
    }
}
