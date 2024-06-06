using System;
using UnityEngine;

[Serializable]
public class PlayerInteracting
{
    [SerializeField] Vector3 adjustingRaycastPosition = new Vector3(0, 0.5f, 0);
    IInteractable _IInteract;
    RaycastHit _hit;
    Transform _playerT;

    public PlayerInteracting(Transform PlayerT){

        _playerT = PlayerT;
    }

    public void Interaction(){

        if (Physics.Raycast(_playerT.position + adjustingRaycastPosition, _playerT.forward, out _hit, 1)){

            if (_hit.collider != null){

                if (_hit.collider.GetComponent<IInteractable>() != null){

                    _IInteract = _hit.collider.GetComponent<IInteractable>();
                    _IInteract.ToInteract();
                }

                else if (_IInteract != null){

                    _IInteract.NoInteraction();
                    _IInteract = null;                    
                }
            }
        }

        else if (_IInteract != null){

            _IInteract.NoInteraction();
            _IInteract = null;
        }

        Debug.DrawRay(_playerT.position + adjustingRaycastPosition, _playerT.forward, Color.green);
    }
}
