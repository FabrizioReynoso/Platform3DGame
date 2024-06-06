using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStartAutomaticMovement : MonoBehaviour
{
    public float speed;

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<CheckFloor>() != null){

            other.GetComponentInParent<Player>().automaticMovement = true;
            other.GetComponentInParent<Player>().speedMovement = speed;
        }
    }
}
