using UnityEngine;

public abstract class PlatformTrap: PlatformMoveable
{
    public bool trapActive = true;

    public override void Start()
    {
        base.Start();
        m_move = false;
    }

    public virtual void DeactivatingTrap(){

        trapActive = false;
    }

    public void ActivatingTrap(){

        GetComponent<Rigidbody>().isKinematic = false;
        m_move = true; 
    }
}
