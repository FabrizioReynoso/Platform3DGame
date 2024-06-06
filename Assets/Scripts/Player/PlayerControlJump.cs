using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerControlJump
{
    public bool jumpActive = true;
    public bool doubleJumpActive;
    public float speedY;
    public bool isCeiling;

    Player _player;
    Animator _playerAnimator;
    Transform _playerTransform;
    PlayerJump _playerJump;
    RaycastHit _hitDown;
    RaycastHit _hitUp;
    bool _isFall;
    bool _reachMaxHeight;
    float _speedJump;
    float _speedImpulse;

    public PlayerControlJump(Player Player){

        _player = Player;
        _playerAnimator = _player.GetComponent<Animator>();
        _playerTransform = _player.GetComponent<Transform>();
        _playerJump = new PlayerJump(_player.GetComponent<Rigidbody>(), _playerTransform);
    }

    public void JumpConditionsControl(float speedJump, float speedImpulse)
    {
        // Control de Salto

        _speedJump = speedJump;
        _speedImpulse = speedImpulse;

        if (Input.GetKeyDown(KeyCode.Space) && jumpActive){

            if (Physics.Raycast(_player.ceilingCollider.position, Vector3.up, out _hitUp, _player.minDistanceToJump)){

                if (_hitUp.collider.isTrigger){

                    OnJump(_speedJump, _speedImpulse);
                    jumpActive = false;
                    doubleJumpActive = true;
                    _player.jumpingSound.Play(); 
                }
            }

            else{

                OnJump(_speedJump, _speedImpulse);
                jumpActive = false;
                doubleJumpActive = true;
                _player.jumpingSound.Play(); 
            }
        }

        // Control de Doble Salto

        else if (Input.GetKeyDown(KeyCode.Space) && doubleJumpActive){

            doubleJumpActive = false;
            speedY = (speedJump + speedImpulse) * 0.75f;    
            _player.jumpingSound.Play();
            
        }    
    }

    public void JumpControl(){

        // Control de Caída

        if (speedY < 0){

            _playerAnimator.SetBool("Jumping", false);
            _isFall = true;

            if (_playerAnimator.GetBool("Landing")){

                _playerAnimator.SetBool("Landing", false);
            }

            // Si el raycast detecta un collider desde la posicion maxima de ascenso, se reconoce la caida

            if (!_reachMaxHeight && !Physics.Raycast(_player.floorCollider.position, Vector3.down, out _hitDown, _player.minDistanceFallToLand)){

                _reachMaxHeight = true;
                _playerAnimator.SetBool("Falling", true);
            }
        }    

        // Si está en el Piso
        if (_player.IsFloor){

            _playerAnimator.SetBool("Falling", false);

            // Si empieza a saltar estando en el piso

            if (_playerAnimator.GetBool("Jumping")){

                _player.IsFloor = false;
            }

            // Si toca el piso al caer

            if (_isFall){

                speedY = 0;
                doubleJumpActive = false;

                if (!_player.automaticMovement){

                    jumpActive = true;
                }

                // Ajuste de animacion si el jugador salta estando cerca de techo

                if (_playerAnimator.GetBool("Jumping")){

                    _playerAnimator.SetBool("Jumping", false);
                }

                // Si la distancia de caida alcanza el valor minimo requerido, se activara el sonido de aterrizaje.

                if (_reachMaxHeight == true){

                    _player.landingSound.Play();
                    _playerAnimator.SetBool("Landing", true);
                    _player.LandingToIdleActive = true;
                }

                _reachMaxHeight = false;
                _isFall = false;
            }   

            // Si la animacion de salto o de caida persiste aun estando en suelo, pasara a la animacion de aterrizaje

            if (jumpActive == true){

                if (!_playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")){

                    AnimatorStateInfo stateInfo = _playerAnimator.GetCurrentAnimatorStateInfo(0);

                    if (stateInfo.IsTag("JumpingUp") || stateInfo.IsTag("FallingIdle")){

                        _playerAnimator.SetBool("Landing", true);
                        _player.LandingToIdleActive = true;
                    }                      
                }
            }
        }   

        // Si toca el Techo
        if (isCeiling && speedY > 0){

            speedY = 0;
            jumpActive = false;
            isCeiling = false;
        }
    }

    public void JumpSpeedControl(float gravity, float maxFallSpeed){

        // Control de Velocidad en Y

        if (_player.IsFloor == false && speedY >= -maxFallSpeed){

            speedY -= gravity * Time.fixedDeltaTime;
        }
    }

    public void MovementControl(){

        if (speedY > 0){

            _playerJump.Jumping(speedY);
        }

        if (speedY < 0){

            _playerJump.Falling(speedY);
        }
    }

    public void OnJump(float speedJump, float speedImpulse){

        speedY = speedJump + speedImpulse;   
        _playerAnimator.SetBool("Jumping", true);  
        _playerAnimator.SetBool("Forward", false);
        _playerAnimator.SetBool("Idle", false);
        _playerAnimator.SetBool("Landing", false);       
    }
}
