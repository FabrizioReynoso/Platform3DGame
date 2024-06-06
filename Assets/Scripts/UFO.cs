using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] float speedRotate;

    void FixedUpdate(){

        transform.eulerAngles += new Vector3(0,1,0) * speedRotate * Time.fixedDeltaTime;
    }
}
