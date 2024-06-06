using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public abstract class PlatformMoveable : Platform
{
    public Vector3 moveDirection;
    public float speed;
    public float acceleration;

    [SerializeField] protected float waitTime = 3f;
    [SerializeField] protected MovementType m_movementType = MovementType.Transform;
    [SerializeField] float amplitude = 10f;

    protected PlatformMovement m_platformMovement;
    protected bool m_move = true;
    protected bool m_fall;

    bool _inFirstHalfCycle = true;
    bool _inSecondHalfCycle;
    float _t;
    float _distance;
    Vector3 _displacement;

    public bool Move{ set{ m_move = value; } }

    public override void Start(){

        base.Start();
        m_platformMovement = new (GetComponent<Rigidbody>());   
    }

    void FixedUpdate(){

        if (m_move){

            switch(m_movementType){

                case MovementType.Transform:

                    m_platformMovement.TransformMovement(moveDirection, speed);
                    break;

                case MovementType.RbMovePosition:

                    m_platformMovement.RbMovement(moveDirection, speed);
                    break;     

                case MovementType.Harmonic:

                    _t += Time.fixedDeltaTime;
                    _distance = amplitude * Mathf.Sin(speed * _t);
                    _displacement = moveDirection * _distance;
                    m_platformMovement.HarmonicMovement(_displacement);

                    if (_t >= Mathf.PI/0.25f && _inFirstHalfCycle == true){

                        m_platformMovement.HarmonicMovement(Vector3.zero);
                        _t = Mathf.PI/0.25f;
                        StartCoroutine(Wait());
                        _inFirstHalfCycle = false;
                        _inSecondHalfCycle = true;
                    }

                    if (_t >= 2*Mathf.PI/0.25 && _inSecondHalfCycle == true){

                        m_platformMovement.HarmonicMovement(Vector3.zero);
                        _t = 0;
                        StartCoroutine(Wait());
                        _inFirstHalfCycle = true;
                        _inSecondHalfCycle = false;
                    }

                    break;        
            }

            if (acceleration != 0){

                speed += acceleration * Time.fixedDeltaTime;
            }
        }

        if (m_fall){

            speed += acceleration * Time.fixedDeltaTime;
            m_platformMovement.RbMovement(new Vector3(0,1f,0), speed);
            Destroy(gameObject, 5);
        }
    }

    public virtual void OnTriggerEnter(Collider other){

        var checkFloor = other.GetComponent<CheckFloor>();

        if (checkFloor){

            m_player.transform.SetParent(transform);

            if (m_move == true && moveDirection.y > 0){

                m_player.SpeedImpulse = speed;
            }

            else{

                m_player.SpeedImpulse = 0;
            }
        }
    }

    public virtual void OnTriggerStay(Collider other){

        var checkFloor = other.GetComponent<CheckFloor>();

        if (checkFloor){

            m_player.transform.SetParent(transform);

            if (m_move == true && moveDirection.y > 0){

                m_player.SpeedImpulse = speed;
            }

            else{

                m_player.SpeedImpulse = 0;
            }
        }
    }

    public virtual void OnTriggerExit(Collider other){

        var checkFloor = other.GetComponent<CheckFloor>();

        if (checkFloor){

            m_player.transform.SetParent(null);
            m_player.SpeedImpulse = 0;
        }
    }

    public IEnumerator Wait(){

        m_move = false;

        yield return new WaitForSeconds(waitTime);

        m_move = true;
    }
}

public enum MovementType{

    Transform,
    RbMovePosition,
    Harmonic
}
