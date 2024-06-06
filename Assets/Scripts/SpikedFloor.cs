using UnityEngine;

public class SpikedFloor : MonoBehaviour
{
    [SerializeField] AudioSource spikedHitSound;
    [SerializeField] float impactForce = 5f;
    [SerializeField] float damage = 1f;

    void OnTriggerEnter(Collider other){

        var player = other.GetComponent<Player>();

        if (player != null){

            if (player.ActiveTrigger){

                spikedHitSound.Play();
                player.TakingDamage(damage);

                if (impactForce != 0){

                    player.playerControlJump.OnJump(impactForce, 0);
                }
            }
        }
    }
}
