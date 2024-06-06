using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlArea : MonoBehaviour
{
    [Header("Control de zoom de camara")]
    [SerializeField] MainCamera _mainCamera;
    [SerializeField] float _newMaxDistance;
    [Header("Opcional: Para desactivar deteccion de un cañon movible")]
    [SerializeField] CannonMovable _cannon;
    [SerializeField] bool checkPoint;

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Player>() != null){

            if (_cannon != null){

                if (_cannon.Detecting){

                    _cannon.Detecting = false;
                    _cannon.GetAngle();
                    _cannon.adjustRotation = true;
                }
            }

            if (checkPoint == true){

                LevelManager.instance.CheckPoint(transform.position);
            }

            _mainCamera.MaxDistance = _newMaxDistance;
        }
    }
}
