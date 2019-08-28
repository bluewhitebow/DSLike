using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public class BreathCollider : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            EnemyStates es = other.GetComponentInParent<EnemyStates>();

            if(es != null)
            {
                es.DoDamage_();
                SpellEffectManager.singleton.UseSpellEffect("onFire", null, es);
            }
        }
    }
}
