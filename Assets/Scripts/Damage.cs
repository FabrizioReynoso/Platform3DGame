using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter(Collision other){

        if (GetComponent<Collider>().isTrigger == false){

            var damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null){

                damageable.TakingDamage(damage);
            }
        }
    }

    void OnTriggerEnter(Collider other){

        if (GetComponent<Collider>().isTrigger){

            var damageable = other.GetComponent<IDamageable>();

            if (damageable != null){

                damageable.TakingDamage(damage);
            }
        }
    }
}
