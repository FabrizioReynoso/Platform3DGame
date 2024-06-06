using System.Collections;
using UnityEngine;

public class PlatformPatrol : PlatformMoveable
{
    public override void Start(){

        base.Start();

        if (m_movementType == MovementType.Harmonic){

            StartCoroutine(Wait());
        }       
    }

    public override void OnTriggerEnter(Collider other){

        base.OnTriggerEnter(other);

        var addressableCollider = other.GetComponent<AddressableCollider>();
        var checkCeiling = other.GetComponent<CheckCeiling>();

        if (checkCeiling && moveDirection.y < 0){

            m_move = false;
            GetComponent<Collider>().isTrigger = true;
        }

        if (addressableCollider){

            moveDirection = addressableCollider.transform.forward;
            StartCoroutine(Wait());
        }
    }

    public override void OnTriggerExit(Collider other){

        base.OnTriggerExit(other);

        if (other.GetComponent<CheckCeiling>() != null){

            m_move = true;
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
