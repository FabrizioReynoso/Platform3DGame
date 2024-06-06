using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject weaponObject;
    [SerializeField] List<MovableGround> movableGrounds;

    LevelManager _levelManager;

    void Start(){

        _levelManager = LevelManager.instance;
    }

    void OnTriggerEnter(Collider other){

        var player = other.GetComponent<Player>();

        if (player){

            player.StopMovement();
            player.ControlsActive = false;
            player.ActiveWeapon = true;
            weaponObject.SetActive(true);
            _levelManager.HUDDeactive();
            StartCoroutine(WaitEvent());
        }
    }

    IEnumerator WaitEvent(){

        foreach (MovableGround ground in movableGrounds){

            ground.move = true;
        }

        gameObject.SetActive(false);

        yield return null;

    }
}
