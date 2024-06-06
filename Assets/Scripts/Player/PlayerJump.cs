using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerJump
{
    Rigidbody _playerRb;
    Transform _playerT;

    public PlayerJump(Rigidbody playerRb, Transform playerT){

        _playerRb = playerRb;
        _playerT = playerT;
    }

    public void Jumping(float speedY){

       _playerT.position += Vector3.up * speedY * Time.fixedDeltaTime;
    }

    public void Falling(float speedY){

        _playerRb.MovePosition(_playerT.position + Vector3.up * speedY * Time.fixedDeltaTime);
    }
}
