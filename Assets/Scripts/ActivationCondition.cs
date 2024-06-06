using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivationCondition : MonoBehaviour
{
    public Action activateEvents;

    [SerializeField] GameObject poster;
    [SerializeField] List<GameObject> objectsRequired;

    public void CheckConditions(){

        if (activateEvents != null){

            int objectsNum = objectsRequired.Count;
            int nullObjectsNum = 0;

            if (objectsNum > 0){

                for (int i=0; i < objectsNum; i++){

                    if (objectsRequired[i] == null){

                        nullObjectsNum++;

                        if (nullObjectsNum == objectsNum){

                            activateEvents.Invoke();
                            Destroy(this); 

                            break;                           
                        }
                    }

                    else{

                        poster.SetActive(true);

                        break;
                    }
                }
            }
        }
    }

    public void DeactivePoster(){

        poster.SetActive(false);
    }
}
