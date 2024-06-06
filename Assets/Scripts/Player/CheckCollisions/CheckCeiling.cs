using UnityEngine;

public class CheckCeiling : MonoBehaviour
{
    [SerializeField] Player player;

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Collider>().isTrigger == false){

           player.IsCeiling = true;
           player.playerControlJump.isCeiling = true;
        }
    }

    void OnTriggerStay(Collider other){

        if (other.GetComponent<Collider>().isTrigger == false){

           player.IsCeiling = true;
           player.playerControlJump.isCeiling = true;
        }
    }    

    void OnTriggerExit(Collider other){

        if (other.GetComponent<Collider>().isTrigger == false){

           player.IsCeiling = true;
           player.playerControlJump.isCeiling = false;
        }
    }
}
