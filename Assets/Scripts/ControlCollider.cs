using UnityEngine;

public class ControlCollider : MonoBehaviour
{
    [SerializeField] ActivationCondition activationCondition;

    void OnTriggerEnter(Collider other){

        var player = other.GetComponent<Player>();

        if (player && activationCondition){

            activationCondition.CheckConditions();
        }
    }

    void OnTriggerExit(Collider other){

        var player = other.GetComponent<Player>();

        if (player && activationCondition){

            activationCondition.DeactivePoster();
        }
    }
}
