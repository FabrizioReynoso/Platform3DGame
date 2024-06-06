using System;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] AudioSource doorOpeningSound;
    [SerializeField] Player player;
    [SerializeField] DoorOpen _doorLeft;
    [SerializeField] DoorOpen _doorRight;
    [SerializeField] GameObject _MessageToInteract;
    [SerializeField] GameObject _MessageClosedDoor;
    [SerializeField] TextMeshProUGUI CountKey;
    [SerializeField] int NumberKeysRequired;

    LevelManager _levelManager;
    Animator _animator;
    bool _interact;

    void Start(){

        _levelManager = LevelManager.instance;
        _animator = player.GetComponent<Animator>();
    }

    void Update(){

        if (_interact && Input.GetKeyDown(KeyCode.Q)){

            EndMessage();
        }
    }

    public void ToInteract(){

        var landing = _animator.GetBool("Landing");
        var isFloor =  player.IsFloor;

        if (!landing && isFloor && _MessageClosedDoor.activeSelf == false){

            _MessageToInteract.SetActive(true);

            if (_interact == false && Input.GetKeyDown(KeyCode.E)){

                Interacting();
            }
        }

        if (!isFloor){

            _MessageToInteract.SetActive(false);
        }
    }

    public void NoInteraction(){

        _MessageToInteract.SetActive(false);
    }

    void Interacting(){

        if (Convert.ToInt32(CountKey.text) >= NumberKeysRequired){

            _doorLeft.open = true;
            _doorRight.open = true;
            _MessageToInteract.SetActive(false);
            _levelManager.CheckPoint(transform.position);
            doorOpeningSound.Play();

            if (_levelManager.messageTutorial){

                _levelManager.messageTutorial.SetActive(false);
            }

            Destroy(GetComponent<BoxCollider>());
            Destroy(this);
        }

        else{

            StartMessage();
        }
    }

    void StartMessage(){

        Time.timeScale = 0;
        _MessageToInteract.SetActive(false);
        _MessageClosedDoor.SetActive(true);
        player.StopMovement();
        player.ControlsActive = false;
        _interact = true;        
    }

    void EndMessage(){

        Time.timeScale = 1f;
        _MessageClosedDoor.SetActive(false);
        _MessageToInteract.SetActive(true);
        player.ControlsActive = true;
        _interact = false;
    }
}
