using UnityEngine;

public class BoxChess : MonoBehaviour
{
    public bool trap;

    [SerializeField] Cannon[] _cannons;

    void CannonFire(bool isFire){

        foreach (Cannon cannon in _cannons){

            cannon.Fire = isFire;
        }            
    }

    void OnTriggerEnter(Collider other){

        if (trap && other.GetComponent<CheckFloor>()){

            CannonFire(true);
        }
    }

    void OnTriggerExit(Collider other){

        if (trap && other.GetComponent<CheckFloor>()){

            CannonFire(false);               
        }
    }
}
