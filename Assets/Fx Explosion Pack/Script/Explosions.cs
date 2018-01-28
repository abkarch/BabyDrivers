using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Explosions : MonoBehaviour
{

    public ParticleSystem explosions;
    public ParticleSystem fire;

    public AudioSource explosionSound;
    public AudioSource babyScream;
	
    public void LowHealthFire()
    {
        fire.Play(true);
    }
    public void SetOffExplosions()
    {
       
        explosions.Play(true);
        explosionSound.Play(0);
        babyScream.Play(0);
       
    }
	
}
