using System.Collections.Generic;
using UnityEngine;

public class PlatformTransport : PlatformMoveable
{
    [Header("Opcion para ajustar la direccion de movimiento")]
    [SerializeField] Vector3 destinyPoint;
    [SerializeField] Transform checkPoint;

    [Header("Asignar colliders de paredes, que sirvan como perímetros, si es necesario para controlar su activacion")]
    [SerializeField] List<Collider> boundaryColliders;
    [SerializeField] bool cameraFixedPositionX;
    [SerializeField] bool cameraFixedPositionY;
    [SerializeField] bool cameraFixedPositionZ;
    [SerializeField] bool playerStop;
    [SerializeField] bool endLevel;

    ActivationCondition _activationCondition;
    MainCamera _mainCamera;
    UFO _ufo;
    Vector3 _initialPosition;
    bool _activateTrigger = true;

    public bool ActivateTrigger{ set{ _activateTrigger = value; } }

    public override void Start(){

        base.Start();

        _activationCondition = GetComponent<ActivationCondition>();
        _mainCamera = MainCamera.instance;
        _ufo = GetComponent<UFO>();
        _initialPosition = transform.position;
        m_move = false;

        if (destinyPoint != Vector3.zero){

            moveDirection = (destinyPoint - transform.position).normalized;
        }

        if (_activationCondition){

            _activateTrigger = false;
            _activationCondition.activateEvents = ComponentActive;
        }
    }

    public override void OnTriggerEnter(Collider other){

        if (_activateTrigger){

            var checkFloor = other.GetComponent<CheckFloor>();

            if (checkFloor && !playerStop){

                if (_mainCamera){

                    _mainCamera.CameraFixedPositionX = cameraFixedPositionX;
                    _mainCamera.CameraFixedPositionY = cameraFixedPositionY;
                    _mainCamera.CameraFixedPositionZ = cameraFixedPositionZ;

                    if (cameraFixedPositionX || cameraFixedPositionY || cameraFixedPositionZ){

                        _mainCamera.checkPlayerVision = true;
                    }
                }

                if (boundaryColliders.Count > 0){

                    foreach (Collider collider in boundaryColliders){

                        collider.enabled = true;
                    }
                }

                if (!_ufo){

                    m_gameManager.OnBlackScreen = Return;
                    m_levelManager.DeathEvent = MovementStop;
                }
            }

            if (checkFloor && playerStop && endLevel){

                if (_mainCamera){

                    _mainCamera.CameraFixedPositionX = cameraFixedPositionX;
                    _mainCamera.CameraFixedPositionY = cameraFixedPositionY;
                    _mainCamera.CameraFixedPositionZ = cameraFixedPositionZ;

                    if (cameraFixedPositionX || cameraFixedPositionY || cameraFixedPositionZ){

                        _mainCamera.checkPlayerVision = true;
                    }
                }
                
                m_levelManager.FinishLevel();
            }
        }

        var addressableCollider = other.GetComponent<AddressableCollider>();

        if (addressableCollider){

            if (addressableCollider.destinyPoint == true){

                addressableCollider.boundaryColliderActive();
                m_player.transform.SetParent(null);
                m_player.SpeedImpulse = 0;   
                m_move = false;
                m_gameManager.OnBlackScreen = null;
                m_levelManager.DeathEvent = null;
                m_levelManager.CheckPoint(checkPoint.position);
                _activateTrigger = false;

                if (_ufo){

                    _mainCamera.transform.parent.SetParent(null);
                    _mainCamera.AdjustCameraRotation(Quaternion.Euler(30f, 0, 0));
                    Destroy(_ufo);
                }  

                if (playerStop){

                    m_player.ControlsActive = true;
                } 

                if (boundaryColliders.Count > 0){

                    foreach (Collider collider in boundaryColliders){

                        collider.gameObject.SetActive(false);
                    }
                }

                if (_mainCamera){

                    _mainCamera.CameraFixedPositionX = false;
                    _mainCamera.CameraFixedPositionY = false;
                    _mainCamera.CameraFixedPositionZ = false;
                    _mainCamera.checkPlayerVision = false;
                    _mainCamera.AdjustCameraPosition = true;
                }

                Destroy(other.gameObject);
            }

            else{

                moveDirection = addressableCollider.transform.forward;
            }
        }
    }

    public override void OnTriggerStay(Collider other){

        if (_activateTrigger){

            var checkFloor = other.GetComponent<CheckFloor>();
            var playerAnim = m_player.Animator;

            if (checkFloor && !playerAnim.GetBool("Falling") && !playerAnim.GetBool("Jumping")){

                m_player.transform.SetParent(transform);    
                m_move = true;     

                if (playerStop){

                    m_player.ControlsActive = false;
                    m_player.StopMovement();
                    m_player.Animator.SetBool("Falling", false);
                    m_player.playerControlJump.speedY = 0;
                    _activateTrigger = false;
                }

                if (_ufo){

                    _ufo.enabled = true;
                    Camera.main.transform.parent.SetParent(transform);
                }

                if (m_move == true && moveDirection.y > 0){

                    m_player.SpeedImpulse = speed;
                }

                else{

                    m_player.SpeedImpulse = 0;
                }
            }
        }
    }

    public override void OnTriggerExit(Collider other){

        if (_activateTrigger){

            var checkFloor = other.GetComponent<CheckFloor>();

            if (checkFloor){

                if (!playerStop){

                    base.OnTriggerExit(other);
                }
            }
        }
    }

    public void Return(){

        m_move = false;
        transform.position = _initialPosition;
        _activateTrigger = true;

        if (_mainCamera){

            _mainCamera.CameraFixedPositionX = false;
            _mainCamera.CameraFixedPositionY = false;
            _mainCamera.CameraFixedPositionZ = false;
            _mainCamera.checkPlayerVision = false;
            _mainCamera.AdjustCameraPosition = true;
        }

        if (boundaryColliders.Count > 0){

            foreach (Collider collider in boundaryColliders){

                collider.gameObject.SetActive(false);
            }
        }
    }

    public void MovementStop(){

        m_move = false;
    }

    public void ComponentActive(){

        _activateTrigger = true;
    }
}
