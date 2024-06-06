using UnityEngine;

[RequireComponent(typeof(Light))]
public class RealTimeLantern : MonoBehaviour
{
    [SerializeField] float maxIntensity;

    Light _light;
    LevelManager _levelManager;

    void Start(){
        
        _levelManager = LevelManager.instance;
        _light = GetComponent<Light>();
        _light.intensity = 0;
    }

    void Update(){

        if (_light.intensity < maxIntensity){

            _light.intensity += 1/(_levelManager.gameTimer*60) * Time.deltaTime;
        }
        
        else{

            _light.intensity = maxIntensity;
        }
    }
}
