using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject ButtonUp;
    [SerializeField] GameObject ButtonDown;
    
    void OnTriggerEnter(Collider other){

        if (other.GetComponent<CheckFloor>() != null){

            //FindObjectOfType<Chess>().DeactivateTrap();
            ButtonUp.SetActive(false);
            ButtonDown.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other){

        if (other.GetComponent<CheckFloor>() != null){

            ButtonUp.SetActive(false);
            ButtonDown.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other){

        if (other.GetComponent<CheckFloor>() != null){

            ButtonUp.SetActive(true);
            ButtonDown.SetActive(false);
        }
    }
}
