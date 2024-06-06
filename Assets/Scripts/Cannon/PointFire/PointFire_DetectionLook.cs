using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFire_DetectionLook : MonoBehaviour
{
    [SerializeField] CannonMovable cannonMovable;

    Player _player;
    RaycastHit hit;

    void Update(){

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50)){

            if (hit.collider != null){

                _player = hit.collider.GetComponent<Player>();

                if (_player != null){

                    cannonMovable.Detecting = true;
                }
            }
        }

        Debug.DrawRay(transform.position, transform.forward * 50, Color.green);
    }
}
