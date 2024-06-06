using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPendulum : MonoBehaviour
{
    public Transform pivotChain;
    public Transform Ball;
    public float angleMax;
    public float Acceleration;

    [SerializeField] Vector3 _moveDirection;
    [SerializeField] float _speedMovement;

    AudioSource _audioSource;
    AddressableCollider _addressableCollider;

    bool _soundActive1 = true;
    bool _soundActive2 = true;
    float _radius;
    float _angularFrequency;
    float _angle;
    float _anglePhase;
    float _t;

    void Start(){

        _audioSource = GetComponent<AudioSource>();
        _radius = pivotChain.position.y - Ball.position.y;
        _angularFrequency = Mathf.Sqrt(Acceleration / _radius);
        _anglePhase = pivotChain.eulerAngles.z;
    }

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<AddressableCollider>() != null){

            _addressableCollider = other.GetComponent<AddressableCollider>();
        }
    }

    void FixedUpdate(){

        if (_addressableCollider == null){

            transform.position += _moveDirection * _speedMovement * Time.fixedDeltaTime;
        }
        
        else{

            transform.position += _addressableCollider.transform.forward * _speedMovement * Time.fixedDeltaTime;
        }
        
        _t += Time.fixedDeltaTime;
        pivotChain.eulerAngles = new Vector3(pivotChain.eulerAngles.x, pivotChain.eulerAngles.y, _angle);
        _angle = angleMax * Mathf.Sin(_angularFrequency * _t + _anglePhase);
    }

    void Update(){

        if (_angle > 0 && _soundActive1 == true){

            _audioSource.Play();
            _soundActive1 = false;
            _soundActive2 = true;
        }

        if (_angle < 0 && _soundActive2 == true){

            _audioSource.Play();
            _soundActive1 = true;
            _soundActive2 = false;
        }
    }
}
