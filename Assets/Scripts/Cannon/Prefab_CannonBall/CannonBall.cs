using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] float damage = 1f;

    AudioSource _audioSource;

    void Start(){

        Destroy(gameObject, 5);
    }

    void OnCollisionEnter(Collision other){

        var otherGameObject = other.gameObject;
        var player = otherGameObject.GetComponent<Player>();
        var collider = otherGameObject.GetComponent<Collider>();

        if (player != null){

            player.cannonBallHit.PlayOneShot(player.cannonBallHit.clip);
            player.TakingDamage(damage);
        }

        if (collider != null){

            if (collider.isTrigger == false){

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other){

        if (other.CompareTag("Cannon")){

            GetComponent<Collider>().isTrigger = false;
        }
    }
}
