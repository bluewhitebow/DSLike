using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public class Projectile : MonoBehaviour
    {
        Rigidbody rigid;


        public float hSpeed = 5;
        public float vSpeed = 2;

        public GameObject explosionPrefab;

        public void Init()
        {
            rigid = GetComponent<Rigidbody>();

            Vector3 targetForce = transform.forward * hSpeed;
            targetForce += transform.forward * vSpeed;
            rigid.AddForce(targetForce, ForceMode.Impulse);
        }

        void OnTriggerEnter(Collider other)
        {

            EnemyStates es = other.GetComponentInParent<EnemyStates>();
            if(es != null)
            {
                es.health -= 40;
                es.DoDamage_();
                SpellEffectManager.singleton.UseSpellEffect("onFire", null, es);
            }
            GameObject go = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
            Destroy(this.gameObject);

        }
    }
}
