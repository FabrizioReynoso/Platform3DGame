using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class LevelManager: MonoBehaviour
{
    public static LevelManager instance;
    public Action DeathEvent;
    public BlackScreen blackScreen;

    [Header("Definir tiempo en minutos")]
    public float gameTimer = 5f;
    public GameObject messageTutorial;

    [SerializeField] Player player;
    [SerializeField] Image greenHealthBar;
    public TextMeshProUGUI lifeCounter;
    [SerializeField] Material skyboxMaterial; 
    [SerializeField] Light directionalLight;
    [SerializeField] Transform clockHand;
    [SerializeField] Transform sunPivot;
    [SerializeField] MainCamera mainCamera;
    [SerializeField] GameObject scene;
    [SerializeField] GameObject deathScreenCamera;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject canvasHUD;
    [SerializeField] GameObject victoryScreen;
    [SerializeField] AudioSource gameSong;
    [SerializeField] AudioSource gameOverSound;
    [SerializeField] AudioSource victorySound;
    [SerializeField] AudioSource pauseSound;
    [SerializeField] AudioSource fatalFallVoice;
    [SerializeField] AudioSource reappearSound;
    [SerializeField] List<AudioSource> reappearVoices;
    [SerializeField] float blackScreenInitialTime;
    [SerializeField] float blackScreenEndTime = 2.5f;
    [SerializeField] float initialExposure = 1.5f;
    [SerializeField] float endExposure = 0.75f;
    [SerializeField] float rotationSpeedSkybox = 0.75f;
    [SerializeField] float fatalFallTimer = 3f;

    GameManager _gameManager;
    Color _ambientLightColor;
    Vector3 _savedPosition;
    System.Random _random;
    bool _timer;
    bool _timerVoice;
    bool _isPlayerStop;
    float _exposure;
    float _rotationSkybox;
    float _opacity = 1f;
    float _clockHandAngleZ;
    float _sunPivotAngleZ;

    void Awake(){

        instance = this;
    }

    void Start(){

        _gameManager = GameManager.instance;
        _random = new System.Random();
        _exposure = initialExposure;
        _ambientLightColor = directionalLight.color;
        _clockHandAngleZ = clockHand.eulerAngles.z;
        _sunPivotAngleZ = sunPivot.eulerAngles.z;
        _gameManager.OffBlackScreen = StartGame;
        MainCamera.instance.AdjustCameraRotation(Quaternion.Euler(30f, 0, 0));
        HUDDeactive();
        PlayerControlsDeactive();
        blackScreen.time = blackScreenInitialTime;
        skyboxMaterial.SetFloat("_Exposure", initialExposure);
        skyboxMaterial.SetFloat("_Rotation", 0);
        canvasHUD.SetActive(false);

        // AL CREAR NIVELES DE OTROS MUNDOS, LO UNICO QUE DIFERENCIAN EN MECANICAS DE NIVEL SERIA LA PENALIZACION POR TIEMPO TRANSCURRIDO
    } 

    void Update(){

        /*
        if (Input.GetKeyDown(KeyCode.P)){

            _isPlayerStop = player.ControlsActive;

            if (Pause.activeSelf == false){

                Time.timeScale = 0;
                player.StopMovement();
                player.ControlsActive = false;
                Pause.SetActive(true);
                pauseSound.Play();
                HUDDeactive();
            }
            
            else if (Pause.activeSelf){

                Time.timeScale = 1f;

                if (!_isPlayerStop){

                    player.ControlsActive = true;
                }

                Pause.SetActive(false);
                HUDActive();
            }
        }
        */

        if (_timer == true){

            if (_opacity > 0){

                _opacity -= 1/(60f * gameTimer) * Time.deltaTime;
                _clockHandAngleZ -= 180f/(60f * gameTimer) * Time.deltaTime;
                _sunPivotAngleZ -= 180f/(60f * gameTimer) * Time.deltaTime;
            }
            
            else{

                _opacity = 0;
                _clockHandAngleZ = -90f;
                _sunPivotAngleZ = 180f;
                _timer = false;
            }

            _exposure = (initialExposure - endExposure) * _opacity + endExposure;
            _rotationSkybox = Time.time * rotationSpeedSkybox;
            skyboxMaterial.SetFloat("_Exposure", _exposure);
            skyboxMaterial.SetFloat("_Rotation", _rotationSkybox);
            directionalLight.color = new Color(_ambientLightColor[0], _ambientLightColor[1], _ambientLightColor[2]) * _opacity;
            clockHand.eulerAngles = new Vector3(clockHand.eulerAngles.x, clockHand.eulerAngles.y, _clockHandAngleZ);
            sunPivot.eulerAngles = new Vector3(sunPivot.eulerAngles.x, sunPivot.eulerAngles.y, _sunPivotAngleZ);
        }

        if (_timerVoice){

            fatalFallVoice.volume -= 1/4f * Time.deltaTime;
        }
    }

    public void StartGame(){

        HUDActive();
        PlayerControlsActive();
        TimerActive();
    }

    public void CheckPoint(Vector3 savedPosition){

        _savedPosition = savedPosition;
    }

    public void HUDActive(){

        canvasHUD.SetActive(true);
    }

    public void HUDDeactive(){

        canvasHUD.SetActive(false);
    }

    public void Dead(){

        if (Convert.ToInt32(lifeCounter.text) > 0){

            _gameManager.OnBlackScreen += CheckPointReturn;
            _gameManager.OnBlackScreen += _gameManager.BlackScreenFade;
            _gameManager.blackScreen.time = 1f;
            _gameManager.blackScreen.appear = true;
        }

        else{

            gameSong.Stop();
            gameOverSound.Play();
            StartCoroutine(OnGameOverSound());
        }
    }

    public void RestartLevel(){

        int indexScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indexScene);
    }

    public void VictoryScreenActive(){

        victoryScreen.SetActive(true);
        blackScreen.gameObject.SetActive(false);
        scene.SetActive(false);
    }

    public void FinishLevel(){

        blackScreen.time = blackScreenEndTime;
        _gameManager.OnBlackScreen = VictoryScreenActive;
        _gameManager.OnBlackScreen += VictorySoundActive;
        _gameManager.BlackScreenAppear();
        player.StopMovement();
        PlayerControlsDeactive();  
        HUDDeactive();  
    }

    void PlayerControlsActive(){

        player.ControlsActive = true;
    }

    void PlayerControlsDeactive(){

        player.ControlsActive = false;
    }

    void TimerActive(){

        _timer = true;
    }

    void CheckPointReturn(){

        mainCamera.enabled = true;
        mainCamera.AdjustCameraPosition = true;
        int randomIndex = _random.Next(0, reappearVoices.Count);
        reappearVoices[randomIndex].Play();
        reappearSound.Play();
        fatalFallVoice.volume = 0.4f;
        lifeCounter.text = (Convert.ToInt32(lifeCounter.text) - 1).ToString();
        player.Animator.SetBool("Death", false);
        player.health = 10;
        player.ActiveTrigger = true;
        player.ControlsActive = true;
        player.speedMovement = player.NormalSpeedMovement;
        player.playerControlJump.speedY = 0;
        player.enabled = true;
        player.transform.position = _savedPosition;
        player.transform.eulerAngles = Vector3.zero;
        player.GetComponent<Collider>().enabled = true;
        player.gameObject.SetActive(true);
        greenHealthBar.fillAmount = 1f; 
        _timerVoice = false;
    }

    void VictorySoundActive(){

        GetComponent<AudioListener>().enabled = true;
        gameSong.Stop();
        victorySound.Play();
    }

    void MainCameraActive(){

        mainCamera.gameObject.SetActive(true);
        deathScreenCamera.SetActive(false);
    }

    void OnApplicationQuit(){

        skyboxMaterial.SetFloat("_Exposure", initialExposure);
        skyboxMaterial.SetFloat("_Rotation", 0);
    }

    public IEnumerator InFatalFall(){

        fatalFallVoice.Play();
        _timerVoice = true;
        mainCamera.enabled = false;

        if (player.spendingEnergy){

            player.spendingEnergy.Stop();
        }

        player.StopMovement();
        player.ControlsActive = false;
        player.WasterEnergy = false;
        HUDDeactive();
        _gameManager.OnBlackScreen += HUDActive;

        if (DeathEvent != null){

            DeathEvent.Invoke();
        }

        yield return new WaitForSeconds(fatalFallTimer);

        Dead();
    }

    public IEnumerator InDeathByBlow(){

        //deathByBlowVoice.Play();
        //_timerVoice = true;

        if (player.spendingEnergy){

            player.spendingEnergy.Stop();
        }
    
        player.Animator.SetBool("Death", true);
        player.ActiveTrigger = false;
        player.StopMovement();
        player.WasterEnergy = false;
        player.enabled = false;
        deathScreenCamera.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        HUDDeactive();
        _gameManager.OnBlackScreen += HUDActive;
        _gameManager.OnBlackScreen += MainCameraActive;

        if (DeathEvent != null){

            DeathEvent.Invoke();
        }

        yield return new WaitForSeconds(fatalFallTimer);

        Dead();
    }

    IEnumerator OnGameOverSound(){

        yield return new WaitForSeconds(4f);
        RestartLevel();
    }
}
