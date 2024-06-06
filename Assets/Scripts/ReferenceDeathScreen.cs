using UnityEngine;

public class ReferenceDeathScreen : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 _distanceVector;

    void Start(){

        _distanceVector = transform.position - player.position;
    }

    void LateUpdate(){

        transform.position = player.position + _distanceVector;
    }
}
