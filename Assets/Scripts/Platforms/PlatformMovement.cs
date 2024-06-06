using System.Collections;
using UnityEngine;

public struct PlatformMovement
{
    Rigidbody _platformRb;
    Transform _platformT;
    Vector3 _initialPosition;

    public PlatformMovement(Rigidbody platformRb){

        _platformRb = platformRb;
        _platformT = _platformRb.transform;
        _initialPosition = _platformT.position;
    }

    public void Stop(){

        _platformRb.velocity = new Vector3(0,0,0);
    }

    public void TransformMovement(Vector3 moveDirection, float speed){

        _platformT.position += moveDirection * speed * Time.fixedDeltaTime;
    }

    public void RbMovement(Vector3 moveDirection, float speed){

        _platformRb.MovePosition(_platformT.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    public void HarmonicMovement(Vector3 displacement){

        _platformT.position = _initialPosition + displacement;
    }
}
