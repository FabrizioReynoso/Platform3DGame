using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] List<Transform> scenePoints;

    LevelManager _levelManager;

    void Start(){

        _levelManager = LevelManager.instance;
    }

    void Update(){

        if (scenePoints.Count > 0){

            if (Input.GetKeyDown(KeyCode.F1)){

                Player.transform.position = scenePoints[0].position;
            }

            else if (Input.GetKeyDown(KeyCode.F2)){

                Player.transform.position = scenePoints[1].position;
            }   

            else if (Input.GetKeyDown(KeyCode.F3)){

                Player.transform.position = scenePoints[2].position;
            }  

            else if (Input.GetKeyDown(KeyCode.F4)){

                Player.transform.position = scenePoints[3].position;
            }   

            else if (Input.GetKeyDown(KeyCode.F5)){

                Player.transform.position = scenePoints[4].position;
            }   

            else if (Input.GetKeyDown(KeyCode.F6)){

                Player.transform.position = scenePoints[5].position;
            }  

            else if (Input.GetKeyDown(KeyCode.F7)){

                Player.transform.position = scenePoints[6].position;
            }  

            else if (Input.GetKeyDown(KeyCode.F8)){

                Player.transform.position = scenePoints[7].position;
            }  
        }  

        if (Input.GetKeyDown(KeyCode.F12)){

            _levelManager.lifeCounter.text = (System.Convert.ToInt32(_levelManager.lifeCounter.text) + 1).ToString();
        }
    }
}
