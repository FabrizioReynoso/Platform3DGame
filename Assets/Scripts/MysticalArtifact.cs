using UnityEngine;

public class MysticalArtifact : MonoBehaviour
{
    [SerializeField] GameObject posterInteractable;
    [SerializeField] GameObject posterArtifactObtained;

    ActivationCondition _activationCondition;
    Player _player;
    bool _on;

    void Start(){

        _player = Player.instance;
        _activationCondition = GetComponent<ActivationCondition>();

        if (_activationCondition){

            _activationCondition.activateEvents = Active;
        }
    }

    void Update(){

        if (_on && Input.GetKeyDown(KeyCode.E)){

            posterArtifactObtained.SetActive(true);
            posterInteractable.SetActive(false);
            _player.StopMovement();
            _player.ControlsActive = false;
            _player.enabled = false;
        }

        if (_on && _player.ControlsActive == false && Input.GetKeyDown(KeyCode.Q)){

            posterArtifactObtained.SetActive(false);
            _player.ControlsActive = true;
            _player.enabled = true;
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other){

        if (other.GetComponent<Player>()){

            if (_activationCondition){

                _activationCondition.CheckConditions();
            }
        }
    }

    void OnTriggerExit(Collider other){

        if (other.GetComponent<Player>()){

            if (_on){

                posterInteractable.SetActive(false);
            }

            if (_activationCondition){

                _activationCondition.DeactivePoster();
            }
        }
    }

    void Active(){

        posterInteractable.SetActive(true);
        _on = true;
    }
}
