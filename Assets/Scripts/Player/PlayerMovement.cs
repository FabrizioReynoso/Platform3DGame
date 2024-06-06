using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerMovement
{
    Rigidbody _playerRB;
    Transform _playerT;

    public PlayerMovement(Rigidbody playerRB, Transform playerT){

        _playerRB = playerRB;
        _playerT = playerT;
    }

    public void Move(Vector3 movementDirection, float speedMovement){

        _playerRB.velocity = movementDirection * speedMovement * Time.fixedDeltaTime; 
    }

    public void Rotate(float angle){

        _playerT.localRotation = Quaternion.Euler(_playerT.localRotation.x, angle, _playerT.localRotation.z);
    }


    public void Stop(){

        _playerRB.velocity = new Vector3(0,0,0);
    }
}
