using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject _pointDestiny;
    [SerializeField] GameObject _MainCamera;
    [SerializeField] GameObject _CameraPortal;
    [SerializeField] float timer = 3f;

    Player _player;
    LevelManager _levelManager;
    ActivationCondition _activationCondition;
    float _timer;
    bool _OnTimer;

    void Start(){

        _levelManager = LevelManager.instance;
        _activationCondition = GetComponent<ActivationCondition>();

        if (_activationCondition){

            GetComponent<Collider>().enabled = false;
            enabled = false;
            _activationCondition.activateEvents = Active;
        }
    }

    void FixedUpdate(){

        if (_OnTimer){

            if (_timer > 0){

                _timer -= Time.fixedDeltaTime;

                if (_timer <= 1f){

                    _player.StopMovement();
                    _player.automaticMovement = false;
                }
            }

            else{

                _MainCamera.SetActive(true);
                _CameraPortal.SetActive(false); 
                _player.playerControlJump.jumpActive = true;
                _player.speedMovement = _player.NormalSpeedMovement;
                _levelManager.CheckPoint(_player.transform.position);
                _levelManager.HUDActive();
                _OnTimer = false;
            }
        }
    }

    void OnTriggerEnter(Collider other){

        Player player = other.GetComponent<Player>();

        if (player){

            _player = player;
            var _pointDestinyT = _pointDestiny.GetComponent<Transform>();
            var playerT = _player.GetComponent<Transform>();
            playerT.position = _pointDestinyT.position;
            playerT.forward = _pointDestinyT.forward;
            player.automaticMovement = true;
            player.playerControlJump.jumpActive = false;
            player.playerControlJump.doubleJumpActive = false;
            player.speedMovement /= 8;
            _levelManager.HUDDeactive();
            CameraTimer();
            GetComponent<AudioSource>().Play();
        }
    }

    public void Active(){

        GetComponent<Collider>().enabled = true;
        enabled = true;        
    }

    void CameraTimer(){

        _MainCamera.SetActive(false);
        _CameraPortal.SetActive(true);
        _timer = timer;
        _OnTimer = true;
    }
}
