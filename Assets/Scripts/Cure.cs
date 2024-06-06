using UnityEngine;

public class Cure : MonoBehaviour
{
    public AudioSource healingSound;
    
    [SerializeField] int _lifePoints;

    void OnTriggerEnter(Collider other){

        Player player = other.GetComponent<Player>();

        if (player){

            player.Healing(_lifePoints, this);
        }
    }
}
