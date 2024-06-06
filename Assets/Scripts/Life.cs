using UnityEngine;

public class Life : MonoBehaviour
{
    public AudioSource lifeSound;

    [SerializeField] int _lifePoints;

    void OnTriggerEnter(Collider other){

        Player player = other.GetComponent<Player>();

        if (player){

            player.Life(_lifePoints, this);
        }
    }
}
