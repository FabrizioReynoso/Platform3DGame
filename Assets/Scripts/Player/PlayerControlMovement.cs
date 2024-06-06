using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlMovement
{
    Player _player;
    Animator _playerAnimator;
    Vector3 _surfaceNormal;
    Vector3 _movementDirection;
    bool _W;
    bool _A;
    bool _S;
    bool _D;

    public PlayerControlMovement(Player player){

        _player = player;
        _playerAnimator = player.GetComponent<Animator>();
    }

    public void GetSurfaceNormal(Vector3 surfaceNormal){

        _surfaceNormal = surfaceNormal;
    }

    public void ControlMovement(){

        // Control Activado
        if (Input.GetKeyDown(KeyCode.W)){

            _W = true;
        }

        if (Input.GetKeyDown(KeyCode.A)){

            _A = true;
        }

        if (Input.GetKeyDown(KeyCode.S)){

            _S = true;
        }

        if (Input.GetKeyDown(KeyCode.D)){

            _D = true;
        }

        // Control Desactivado
        if (Input.GetKeyUp(KeyCode.W)){

            _W = false;
        }

        if (Input.GetKeyUp(KeyCode.A)){

            _A = false;
        }

        if (Input.GetKeyUp(KeyCode.S)){

            _S = false;
        }

        if (Input.GetKeyUp(KeyCode.D)){

            _D = false;
        }  
    }

    public void StopControlMovement(){

        _W = false;  
        _A = false; 
        _S = false;
        _D = false;    
        _player.playerMovement.Stop();
        _playerAnimator.SetBool("Forward", false);

        if (_playerAnimator.GetBool("Jumping") == false && _playerAnimator.GetBool("Falling") == false && _playerAnimator.GetBool("Landing") == false && _player.IsFloor){

            _playerAnimator.SetBool("Idle", true);
        }
    }

    public void Movement(float speed){

        Vector3 forward = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 right = Vector3.right;
        Vector3 left = Vector3.left;

        if (_player.automaticMovement){

            _movementDirection = Vector3.ProjectOnPlane(forward, _surfaceNormal).normalized;
            _player.playerMovement.Move(_movementDirection, speed);
            _playerAnimator.SetBool("Forward", true);
            _playerAnimator.SetBool("Idle", false);
        }

        if (_player.automaticMovement == false){

            // Movimiento de 4 direcciones primarias
            if (_player.primaryMovementActive){

                if (_W == true && _A == false && _D == false){

                    _movementDirection = Vector3.ProjectOnPlane(forward, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(0);
                }

                if (_A == true && _W == false && _S == false){

                    _movementDirection = Vector3.ProjectOnPlane(left, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(-90f);
                }

                if (_S == true && _A == false && _D == false){

                    _movementDirection = Vector3.ProjectOnPlane(back, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(180f);
                }

                if (_D == true && _W == false && _S == false){

                    _movementDirection = Vector3.ProjectOnPlane(right, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(90f);
                }
            }

            // Movimiento de 4 direcciones secundarias
            if (_player.secondaryMovementActive){

                if (_W == true && _A == true && _S == false && _D == false){

                    _movementDirection = Vector3.ProjectOnPlane(forward + left, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(-45f);
                }      

                if (_W == true && _A == false && _S == false && _D == true){

                    _movementDirection = Vector3.ProjectOnPlane(forward + right, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(45f);
                }

                if (_W == false && _A == true && _S == true && _D == false){

                    _movementDirection = Vector3.ProjectOnPlane(back + left, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(-135f);
                }

                if (_W == false && _A == false && _S == true && _D == true){

                    _movementDirection = Vector3.ProjectOnPlane(back + right, _surfaceNormal).normalized;
                    _player.playerMovement.Move(_movementDirection, speed);
                    _player.playerMovement.Rotate(135f);
                }
            }

            // Sin Movimiento
            if (_W == false && _A == false && _S == false && _D == false){

                StopControlMovement();
            }

            else if (_player.IsFloor && _playerAnimator.GetBool("Jumping") == false){

                _playerAnimator.SetBool("Forward", true);
                _playerAnimator.SetBool("Idle", false);
            }

            if (_player.IsFloor == false){

                _playerAnimator.SetBool("Idle", false);
            }
        }
    }
}
