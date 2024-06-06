using UnityEngine;

public class MotionControllerEffect : MonoBehaviour
{
    [SerializeField] Effect motionController;

    [Header("Case: Vertical Oscillator")]
    [SerializeField] float maxDisplacementY;

    [Header("Case: Vertical Oscillator")]
    [SerializeField] float angularFrequency;

    [Header("Case: Constant Rotation")]
    [SerializeField] Vector3 rotationAxis;

    [Header("Case: Constant Rotation")]
    [SerializeField] float speedRotation;

    float _displacementY;
    float _initialPositionY;
    float _t;
    float _angle;

    void Start(){

        switch (motionController){

            case Effect.VerticalOscillator:

                _initialPositionY = transform.position.y;

                break;
        }
    }

    void FixedUpdate(){

        switch (motionController){

            case Effect.VerticalOscillator:

                _t += Time.fixedDeltaTime;
                _displacementY = maxDisplacementY * Mathf.Sin(angularFrequency * _t);
                transform.position = new Vector3(transform.position.x, _initialPositionY + _displacementY, transform.position.z);

                break;

            case Effect.ConstantRotation:

                _angle = speedRotation * Time.fixedDeltaTime;
                transform.eulerAngles += rotationAxis * speedRotation * Time.fixedDeltaTime;

                break;
        }
    }
}

enum Effect{

    VerticalOscillator,
    ConstantRotation
}
