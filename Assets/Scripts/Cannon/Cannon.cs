using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cannon : MonoBehaviour
{
    [Header("La dirección del CannonBall tomará la del PointFire")]

    [SerializeField] protected Transform PointFire_T;
    [SerializeField] protected GameObject Prefab;
    [SerializeField] protected ParticleSystem _particleSmoke;

    public float cooldown;
    public float velocityBall;

    protected bool fire;
    protected float time;

    public bool Fire{ get{ return fire; } set{ fire = value; } }

    public virtual void Update(){

    }

    public virtual void FixedUpdate(){

        if (fire){

            if (time > 0){

                time -= Time.fixedDeltaTime;
            }

            else{

                GameObject cannonBall = Instantiate(Prefab, PointFire_T.position, PointFire_T.rotation);
                cannonBall.GetComponent<Rigidbody>().velocity = velocityBall * PointFire_T.forward;
                _particleSmoke.Play();
                GetComponent<AudioSource>().Play();
                time = cooldown;            
            }
        }

        else{

            time = 0; 
        }
    }
}


