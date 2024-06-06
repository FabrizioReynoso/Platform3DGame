using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableGround : MonoBehaviour
{
    public bool move;

    [SerializeField] Transform botT;
    [SerializeField] Vector3 movementDirection;
    [SerializeField] float movementSpeed;
    [SerializeField] float waitTime = 3f;
    [SerializeField] bool eventActive;

    LevelManager _levelManager;

    void Start(){

        _levelManager = LevelManager.instance;
    }

    void FixedUpdate(){

        if (move){

            transform.position += movementDirection * movementSpeed * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other){

        var addressableCollider = other.GetComponent<AddressableCollider>();

        if (addressableCollider){

            if (addressableCollider.destinyPoint){

                move = false;

                if (eventActive){

                    _levelManager.HUDActive();

                    var player = Player.instance;

                    player.ControlsActive = true;

                    if (botT){

                        botT.SetParent(null);
                        botT.GetComponent<Bot>().detection = true;
                        botT.GetComponent<Bot>().MoveTowardPlayer();
                    }
                }
            }

            else{

                movementDirection = addressableCollider.transform.forward;
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait(){

        move = false;

        yield return new WaitForSeconds(waitTime);

        move = true;
    }
}
