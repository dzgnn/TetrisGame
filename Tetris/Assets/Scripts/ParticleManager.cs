using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
   public ParticleSystem[] allEffects;

   private void Start()
   {
        allEffects = GetComponentsInChildren<ParticleSystem>();
   }

   public void EffectPlay()
   {
    foreach (ParticleSystem item in allEffects)
    {
        item.Stop();
        item.Play();
    }
   }
}
