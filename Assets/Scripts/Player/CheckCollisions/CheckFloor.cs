using UnityEngine;

public class CheckFloor : MonoBehaviour
{
    [SerializeField] Player player;

    LevelManager _levelManager;

    void Start(){

        _levelManager = LevelManager.instance;
    }

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Collider>().isTrigger == false){

            player.IsFloor = true;

            if (!_levelManager.blackScreen.fade && !_levelManager.blackScreen.appear){

                player.ControlsActive = true; // Al caer al suelo luego de una colision contra un objeto dañable, aun cuando no termina el tiempo de espera.
            }
        }
    }

    void OnTriggerStay(Collider other){

        if (other.GetComponent<Collider>().isTrigger == false){

            player.IsFloor = true;
        } 
    }

    void OnTriggerExit(Collider other){

        if (other.GetComponent<Collider>().isTrigger == false){

            player.IsFloor = false;
        } 
    }
}
