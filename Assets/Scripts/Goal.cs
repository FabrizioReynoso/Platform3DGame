using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool activeTrigger = false;

    LevelManager _levelManager;

    void Start(){
        
        _levelManager = LevelManager.instance;
    }

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Player>() && activeTrigger){

            GetComponent<AudioSource>().Play();
            _levelManager.FinishLevel();
        }
    }
}
