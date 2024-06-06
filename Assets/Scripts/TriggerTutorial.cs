using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    [SerializeField] GameObject poster;

    void OnTriggerEnter(Collider other){

        var player = other.GetComponent<Player>();

        if (player){

            poster.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other){

        var player = other.GetComponent<Player>();

        if (player){

            poster.SetActive(false);
        } 
    }

}
