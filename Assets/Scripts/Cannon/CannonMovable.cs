using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMovable : Cannon
{
    public float speedMovement;
    public float speedRotation;

    Vector3 _currentRotation;

    float _speedAux;
    float _angleRotationDelta;
    float _initialAngle;
    float _endAngle;
    float _currentAngle;
    bool _adjustRotation;
    bool _activeTriggerEnter;
    bool _detecting;

    Transform _playerT;
    AddressableCollider _addressableCollider;

    public bool Detecting{ set{ _detecting = value; } get{ return _detecting; } }

    public bool adjustRotation{ set{ _adjustRotation = value; } }

    void Start(){

        _playerT = Player.instance.transform;
        _activeTriggerEnter = true;
        _speedAux = speedMovement;
        _currentRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<AddressableCollider>() != null && _activeTriggerEnter){

            _currentRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            _speedAux = 0;
            _addressableCollider = other.GetComponent<AddressableCollider>();
            _addressableCollider.OnRotation = true;
        }
    }

    public void GetAngle(){

        _initialAngle = transform.eulerAngles.y;
        _endAngle = _addressableCollider.EndAngle;
        int ki = (int)(_initialAngle / 360f);
        int ke = (int)(_endAngle / 360f);
        _initialAngle -= 360f * ki;
        _endAngle -= 360f * ke;

        if (_initialAngle < 0){

            _initialAngle += 360f;
        }

        if (_endAngle < 0){

            _endAngle += 360f;
        }

        _currentAngle = _initialAngle;

        if (_endAngle - _initialAngle > 0){

            if (_endAngle - _initialAngle > 180f){

                _angleRotationDelta = _endAngle - _initialAngle - 360f;
            }

            else{

                _angleRotationDelta = _endAngle - _initialAngle;
            }
        }

        else if (_endAngle - _initialAngle < 0){

            if (_endAngle - _initialAngle < -180f){

                _angleRotationDelta = _endAngle - _initialAngle + 360f;
            }

            else{

                _angleRotationDelta = _endAngle - _initialAngle;
            }
        }
    }

    void AdjustRotation(){

        if (_angleRotationDelta > 0){

            if (_currentAngle < _endAngle){

                _currentAngle += speedRotation * Time.fixedDeltaTime;
            }

            if (_currentAngle >= _endAngle){

                _currentAngle = _endAngle;
            }
        }

        if (_angleRotationDelta < 0){

            if (_currentAngle > _endAngle){

                _currentAngle -= speedRotation * Time.fixedDeltaTime;
            }

            if (_currentAngle <= _endAngle){

                _currentAngle = _endAngle;
            }
        }

        if (_currentAngle == _endAngle){

            _adjustRotation = false;
            _speedAux = speedMovement;
            _activeTriggerEnter = true;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _currentAngle, transform.eulerAngles.z);
    }

    public override void FixedUpdate(){

        if (_addressableCollider == null){

            transform.position += PointFire_T.forward *  _speedAux * Time.fixedDeltaTime;
        }

        if (_addressableCollider != null){

            transform.position += _addressableCollider.transform.forward *  _speedAux * Time.fixedDeltaTime;

            if (_detecting == false){

                if (_addressableCollider.OnRotation){

                    _addressableCollider.OnOrientedRotation(transform, _currentRotation, "y", speedRotation);
                }

                if (_adjustRotation){

                    AdjustRotation();
                }
            }
        }

        if (fire){

            if (time > 0){

                time -= Time.fixedDeltaTime;
            }

            else{

                GameObject cannonBall = Instantiate(Prefab, PointFire_T.position, PointFire_T.rotation);
                cannonBall.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
                cannonBall.GetComponent<Rigidbody>().velocity = velocityBall * PointFire_T.forward;
                _particleSmoke.Play();
                GetComponent<AudioSource>().Play();
                time = cooldown;            
            }
        }
    }

    public override void Update(){

        if (_detecting){

            _activeTriggerEnter = false;
            _speedAux = 0;
            var vectorLookObject = transform.position - _playerT.position;
            transform.forward = new Vector3(vectorLookObject.x, 0, vectorLookObject.z);
            _addressableCollider.OnRotation = false;
            _addressableCollider.CurrentAngle = _addressableCollider.AngleAux;

            if (_addressableCollider != null){

                _addressableCollider.OnRotation = false;
            }

            fire = true;
        }

        if (_detecting == false){

            fire = false;

            if (_addressableCollider != null){

                if (_addressableCollider.OnRotation == false && _adjustRotation == false){

                    _speedAux = speedMovement;
                }
            }
        }
    }
}
