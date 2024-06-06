using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumBall : MonoBehaviour
{
    [SerializeField] AudioSource spikedHitSound;
    [SerializeField] float impactForce = 100f;
    [SerializeField] float damage = 3f;

    Player _player;
    Rigidbody _playerRb;
    bool _colliderActive;

    void Start(){

        _colliderActive = true;
    }

    void OnCollisionEnter(Collision other){

        Player player = other.gameObject.GetComponent<Player>();

        if (player && _colliderActive){

            if (player.ActiveTrigger){

                spikedHitSound.Play();
                _player = player;
                _playerRb = _player.GetComponent<Rigidbody>();
                _player.ControlsActive = false;
                _player.StopMovement();

                Vector3 surfaceNormal = - other.contacts[0].normal.normalized;

                _playerRb.AddForce(surfaceNormal * impactForce, ForceMode.Impulse);
                _player.TakingDamage(damage);
                _player.playerControlJump.speedY = 0 ;

                StartCoroutine(CooldownCollider());

                _colliderActive = false;
            }
        }
    }

    IEnumerator CooldownCollider(){

        yield return new WaitForSeconds(0.5f);
        _player.ControlsActive = true;
        _colliderActive = true;
    }
}
