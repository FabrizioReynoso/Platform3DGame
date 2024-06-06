using UnityEngine;

public class ChessInteraction : MonoBehaviour
{
    [SerializeField] GameObject _cameraChess;
    [SerializeField] GameObject _messageToInteract;
    [SerializeField] GameObject _messageExit;

    LevelManager _levelManager;
    bool _interacting;

    void Start(){

        _levelManager = LevelManager.instance;
    }

    void OnTriggerStay(Collider other){

        Player player = other.GetComponent<Player>();

        if (player){

            if (player.IsFloor && !player.Animator.GetBool("Landing") && !player.Animator.GetBool("Falling")){

                if (_interacting == false){

                    _messageToInteract.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E)){

                        _messageToInteract.SetActive(false);
                        _messageExit.SetActive(true);
                        _cameraChess.SetActive(true);
                        _levelManager.HUDDeactive();
                        player.StopMovement();
                        player.ControlsActive = false;
                        _interacting = true;
                    }
                }

                if (_interacting == true && Input.GetKeyDown(KeyCode.Q)){

                    _messageExit.SetActive(false);
                    _cameraChess.SetActive(false);
                    _levelManager.HUDActive();
                    player.ControlsActive = true;
                    _interacting = false;
                }
            }

            if (!player.IsFloor || player.Animator.GetBool("Landing") || player.Animator.GetBool("Falling")) {

                _messageToInteract.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other){

        Player player = other.GetComponent<Player>();

        if (player){

            _messageToInteract.SetActive(false);
        }
    }
}
