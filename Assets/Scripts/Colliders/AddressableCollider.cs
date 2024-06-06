using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressableCollider : MonoBehaviour
{
    [Header("Assign a value to AngleRotationDelta between (-360, 360)")]
    public float AngleRotationDelta;
    public bool destinyPoint;

    [SerializeField] Collider boundaryCollider;

    float _currentAngle;
    float _angleAux;
    float _endAngle;
    bool _getAngle = true;
    bool _OnRotation;

    public float CurrentAngle{ set{ _currentAngle = value; } }

    public float AngleAux{ get{ return _angleAux; } }

    public float EndAngle{ get{ return _endAngle; } }

    public bool OnRotation{ get{ return _OnRotation; } set{ _OnRotation = value; } }

    void Start(){

        if (destinyPoint == true){

            AngleRotationDelta = 0;
        }
    }

    public void OnOrientedRotation(Transform Object_T, Vector3 CurrentRotation, string Axis, float speedRotation){

        if (Axis == "y" || Axis == "Y"){

            // SI EL OBJETO TIENE UN ANGULO DIFERENTE DE LA ANTERIOR AL COLISIONAR CON ESTE OBJETO, SU ANGULO INICIAL DE ROTACION SERA EL ANGULO TOMADO ANTERIORMENTE 
            if (_getAngle){

                _currentAngle = CurrentRotation.y;
                _angleAux = _currentAngle;
                _endAngle = CurrentRotation.y + AngleRotationDelta;
                int k = (int)(_currentAngle / 360f);
                _currentAngle -= 360f * k;

                if (_currentAngle < 0){

                    _currentAngle += 360f;
                }

                _getAngle = false;
            }

            if (_getAngle == false){

                if (AngleRotationDelta > 0){

                    if (_currentAngle < _endAngle){

                        _currentAngle += speedRotation * Time.fixedDeltaTime;
                    }

                    if (_currentAngle >= _endAngle){

                        _currentAngle = _endAngle;
                    }
                }

                if (AngleRotationDelta < 0){

                    if (_currentAngle > _endAngle){

                        _currentAngle -= speedRotation * Time.fixedDeltaTime;
                    }

                    if (_currentAngle <= _endAngle){

                        _currentAngle = _endAngle;
                    }
                }

                if (_OnRotation){

                    Object_T.eulerAngles = new Vector3(Object_T.eulerAngles.x, _currentAngle, Object_T.eulerAngles.z);
                }

                if (_currentAngle == _endAngle){

                    _currentAngle = _angleAux;
                    _OnRotation = false;
                }
            }
        }
    }

    public void boundaryColliderActive(){

        if (boundaryCollider != null){

            boundaryCollider.enabled = true;
        }
    }
}
