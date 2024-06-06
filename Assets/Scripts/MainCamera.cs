using UnityEngine;

public class MainCamera: MonoBehaviour
{
    public static MainCamera instance;
    public bool checkPlayerVision;

    [SerializeField] Transform player;
    [SerializeField] float maxDistance;
    [SerializeField] float speedDollyZoom;
    [SerializeField] float rotationSpeed;

    Camera _mainCamera;
    LevelManager _levelManager;
    [SerializeField] Quaternion _endRotation;
    Transform _objectT;
    Vector3 _direction;
    Vector3 _distanceVector;
    Vector3 _cameraPositionAuxiliar;
    RaycastHit _hit;

    bool _cameraFixedPositionX;
    bool _cameraFixedPositionY;
    bool _cameraFixedPositionZ;
    bool _adjustCameraPosition;
    [SerializeField] bool _adjustCameraRotation;

    public bool CameraFixedPositionX{ set{ _cameraFixedPositionX = value; } }
    public bool CameraFixedPositionY{ set{ _cameraFixedPositionY = value; } }
    public bool CameraFixedPositionZ{ set{ _cameraFixedPositionZ = value; } }
    public bool AdjustCameraPosition{ set{ _adjustCameraPosition = value; } }
    public float MaxDistance{ set{ maxDistance = value; } }

    void Awake(){

        instance = this;
    }

    void Start(){

        _mainCamera = GetComponent<Camera>();
        _levelManager = LevelManager.instance;
        _objectT = player;
        _distanceVector = transform.parent.position - player.position;
        _direction = transform.localPosition.normalized;
    }

    void FixedUpdate(){

        if (!checkPlayerVision){

            _cameraPositionAuxiliar = transform.parent.TransformPoint(_direction * maxDistance);

            if (Physics.Linecast(transform.parent.position, _cameraPositionAuxiliar, out _hit)){

                if (!_hit.collider.isTrigger && !_hit.collider.CompareTag("BoundaryCollider")){

                    transform.localPosition = Vector3.Lerp(transform.localPosition, _direction * _hit.distance, speedDollyZoom * Time.deltaTime);
                }

                else{

                    transform.localPosition = Vector3.Lerp(transform.localPosition, _direction * maxDistance, speedDollyZoom * Time.deltaTime);
                }
            }

            else{

                transform.localPosition = Vector3.Lerp(transform.localPosition, _direction * maxDistance, speedDollyZoom * Time.deltaTime);
            }
        }

        if (_adjustCameraRotation){

            if (transform.rotation != _endRotation || transform.parent.rotation != Quaternion.Euler(0,0,0)){

                transform.rotation = Quaternion.Slerp(transform.rotation, _endRotation, rotationSpeed * Time.fixedDeltaTime);
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.Euler(0,0,0), rotationSpeed * Time.fixedDeltaTime);
                Debug.Log("Ejecutando rotacion de camara");
            }

            else{
    
                transform.rotation = _endRotation;   
                transform.parent.rotation = Quaternion.Euler(0,0,0);   
                _adjustCameraRotation = false;
            }
        }

        Debug.DrawRay(transform.parent.position, _cameraPositionAuxiliar - transform.parent.position, Color.red);
    }

    void Update(){

        if (_adjustCameraPosition == true){

            _cameraFixedPositionX = false;
            _cameraFixedPositionY = false;
            _cameraFixedPositionZ = false;
            transform.parent.position = Vector3.Lerp(transform.parent.position, _objectT.position + _distanceVector, speedDollyZoom);
            _adjustCameraPosition = false;
        }
    }

    void LateUpdate(){

        if (!_adjustCameraPosition){

            var newPosition = _objectT.position + _distanceVector;

            if (!_cameraFixedPositionX && !_cameraFixedPositionY && !_cameraFixedPositionZ){

                transform.parent.position = newPosition;
            }
            
            else if (_cameraFixedPositionX && !_cameraFixedPositionY && !_cameraFixedPositionZ){

                transform.parent.position = new Vector3(transform.parent.position.x, newPosition.y, newPosition.z);
            }

            else if (!_cameraFixedPositionX && !_cameraFixedPositionY && _cameraFixedPositionZ){

                transform.parent.position = new Vector3(newPosition.x, newPosition.y, transform.parent.position.z);
            } 

            else if (!_cameraFixedPositionX && _cameraFixedPositionY && _cameraFixedPositionZ){

                transform.parent.position = new Vector3(newPosition.x, transform.parent.position.y, transform.parent.position.z);
            } 

            else if (_cameraFixedPositionX && !_cameraFixedPositionY && _cameraFixedPositionZ){

                transform.parent.position = new Vector3(transform.parent.position.x, newPosition.y, transform.parent.position.z);
            } 
        }

        if (checkPlayerVision){

            if (!ObjectVision()){

                _levelManager.StartCoroutine(_levelManager.InFatalFall()); 
                player.GetComponent<Player>().enabled = false;
                checkPlayerVision = false;
            }
        }
    }

    public void AdjustCameraRotation(Quaternion endRotation){

        _endRotation = endRotation;
        _adjustCameraRotation = true;
    }

    bool ObjectVision(){

        Vector3 viewPosition = _mainCamera.WorldToViewportPoint(player.position);
        return viewPosition.x > -0.15 && viewPosition.x < 1.15 && viewPosition.y > -1;
    }
}
