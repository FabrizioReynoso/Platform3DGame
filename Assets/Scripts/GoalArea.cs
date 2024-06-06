using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalArea : MonoBehaviour
{   
    [SerializeField] GameObject _messageToInteract;
    [SerializeField] Goal _goalPortal;
    [SerializeField] TextMeshProUGUI _countRelics;
    [SerializeField] int _numberRelicsRequired;

    void OnTriggerStay(Collider other){

        if (Convert.ToInt32(_countRelics.text) < _numberRelicsRequired){

            if (other.GetComponent<Player>()){

                _messageToInteract.SetActive(true);
            }
        }

        else{

            _goalPortal.activeTrigger = true;
            _messageToInteract.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other){

        if (other.GetComponent<Player>() != null){

            _messageToInteract.SetActive(false);
        }
    }
}
