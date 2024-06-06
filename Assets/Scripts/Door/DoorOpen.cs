using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public bool open;

    [SerializeField] Vector3 _moveDirection;
    [SerializeField] float _speed;

    void OnTriggerEnter(Collider other){

        if (other.CompareTag("ColliderDoor")){

            _speed = 0;
            open = false;
        }
    }

    void OnTriggerStay(Collider other){

        if (other.CompareTag("ColliderDoor")){

            _speed = 0;
            open = false;
        }
    }

    void FixedUpdate(){

        if (open){

            transform.position += _moveDirection * _speed * Time.fixedDeltaTime;
        }
    }
}
