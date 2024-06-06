using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInitial : PlatformMoveable
{
    public override void Start()
    {
        base.Start();
    }

    public override void OnTriggerEnter(Collider other){

        base.OnTriggerEnter(other);

        var addressableCollider = other.GetComponent<AddressableCollider>();

        if (addressableCollider){

            if (addressableCollider.destinyPoint == true){

                m_move = false;
                m_player.transform.SetParent(null);   
                m_platformMovement.Stop();
                m_levelManager.CheckPoint(m_player.transform.position);
                Destroy(other.gameObject);    
            }
        }     
    }
}
