using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Rigidbody))]

public class Player : MonoBehaviour, IDamageable
{
    public static Player instance;
    public AudioSource jumpingSound;
    public AudioSource landingSound;
    public AudioSource cannonBallHit;
    public PlayerControlMovement playerControlMovement;
    public PlayerMovement playerMovement;
    public PlayerControlJump playerControlJump;
    public Transform floorCollider;
    public Transform ceilingCollider;
    public bool primaryMovementActive;
    public bool secondaryMovementActive;
    public bool automaticMovement;
    public float speedMovement;
    public float health;
    public float minDistanceToJump = 0.25f;
    public float minDistanceFallToLand = 0.25f;
    public float fallMaxTimer = 5f;
    public Image yellowEnergyBar;

    [SerializeField] PlayerInteracting playerInteracting;
    [SerializeField] Image greenHealthBar;
    [SerializeField] TextMeshProUGUI lifeCounter;
    public AudioSource spendingEnergy;
    [SerializeField] AudioSource emptyEnergy;
    [SerializeField] List<AudioSource> hurtVoices;
    [SerializeField] ParticleSystem particleBlood;
    [SerializeField] ParticleSystem particleHealth;
    [SerializeField] Collider weapon;
    [SerializeField] BatWeapon batWeapon;
    //[SerializeField] bool doubleJumpActive;
    [SerializeField] bool activeWeapon;
    [SerializeField] float speedJump;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float gravity;
    [SerializeField] float fallMaxDistance = 5f;
    [SerializeField] float timeLandingToIdle = 0.5f;
    [SerializeField] [Range(0, 10f)] float energyDrainPerSecond = 2f;
    [SerializeField] [Range(0, 10f)] float energyRechargePerSecond = 2f;
    [SerializeField] float timeAttack1;
    [SerializeField] float timeAttack2;
    [SerializeField] float timeAttack3;

    GameManager _gameManager;
    LevelManager _levelManager;
    Transform _transform;
    Animator _animator;
    RaycastHit _hit1;
    RaycastHit _hit2;
    Vector3 _surfaceNormal;
    System.Random _random;
    bool _controlsActive;
    bool _isFloor;
    bool _isCeiling;
    bool _landingToIdleActive;
    bool _timerFallActivate;
    bool _checkFatalFall = true;
    bool _bootsActivate;
    bool _rechargeEnergy;
    bool _wasteEnergy;
    bool _exhausted;
    bool _hurtVoiceActivate = true;
    bool _activeTrigger = true;
    float _normalSpeedMovement;
    float _speedImpulse;
    float _maxHealth;
    float _timeAuxiliar;
    float _timerFall;
    float _timerAttack;
    int _randomIndex;
    int _attackHit;

    public Animator Animator{ get { return _animator; } }
    public bool ControlsActive{ get { return _controlsActive; } set{ _controlsActive = value; } }
    public bool IsFloor{ get{ return _isFloor; } set{ _isFloor = value; } }
    public bool IsCeiling{ get{ return _isCeiling; } set{ _isCeiling = value; } }
    public bool LandingToIdleActive{ get{ return _landingToIdleActive; } set{ _landingToIdleActive = value; } }
    public bool BootsActivate{ set{ _bootsActivate = value; } }
    public bool WasterEnergy{ set{ _wasteEnergy = value; } }
    public bool ActiveWeapon{ set{ activeWeapon = value; } }
    public bool ActiveTrigger{ get{ return _activeTrigger; } set{ _activeTrigger = value; } }
    //public bool CheckFatalFall{ set{ _checkFatalFall = value; } }
    public float NormalSpeedMovement{ get{ return _normalSpeedMovement; } }
    public float SpeedImpulse{ get{ return _speedImpulse; } set{ _speedImpulse = value; } }

    void Awake(){

        instance = this;
        _transform = transform;
        _animator = GetComponent<Animator>();
        playerInteracting = new PlayerInteracting(_transform);
        playerControlMovement = new PlayerControlMovement(this);
        playerMovement = new PlayerMovement(GetComponent<Rigidbody>(), _transform);
        playerControlJump = new PlayerControlJump(this);
    }

    void Start(){

        _gameManager = GameManager.instance;
        _levelManager = LevelManager.instance;
        _random = new System.Random();
        StopMovement();
        _normalSpeedMovement = speedMovement;
        _maxHealth = health;
        _timeAuxiliar = timeLandingToIdle;
        _timerFall = fallMaxTimer;
    }

    void FixedUpdate(){

        playerControlJump.JumpSpeedControl(gravity, maxFallSpeed);
        playerControlJump.MovementControl();
        playerInteracting.Interaction();

        if (_controlsActive){

            playerControlMovement.Movement(speedMovement);
        }

        if (Physics.Raycast(floorCollider.position, Vector3.down, out _hit1, minDistanceFallToLand)){

            // Obtencion del vector normal de la superficie detectada
            _surfaceNormal = _hit1.normal;
            playerControlMovement.GetSurfaceNormal(_surfaceNormal);

        }

        if (_checkFatalFall){

            if (Physics.Raycast(floorCollider.position - new Vector3(0, 0.075f, 0), Vector3.down, out _hit2, fallMaxDistance)){

                if (_hit2.collider){

                    if (_timerFallActivate == true && !_hit2.collider.GetComponent<Player>()){

                        _timerFallActivate = false;
                        _timerFall = fallMaxTimer;
                    }
                }
            }

            else if (_timerFallActivate == false && _isFloor == false){

                _timerFallActivate = true;
            }
        }
    }

    void Update(){

        playerControlMovement.ControlMovement();
        playerControlJump.JumpControl();

        if(_controlsActive){

            playerControlJump.JumpConditionsControl(speedJump, _speedImpulse);

            if (_timerFallActivate == true){

                if (_timerFall > 0){

                    _timerFall -= Time.deltaTime;
                }

                else{
  
                    _timerFallActivate = false;
                    _timerFall = fallMaxTimer;      
                    _controlsActive = false;     
                    _levelManager.StartCoroutine(_levelManager.InFatalFall());   
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && (_animator.GetBool("Idle") || _animator.GetBool("Forward")) && _attackHit < 3 && activeWeapon){

                _animator.SetBool("Attack", true);
                
                if (_attackHit == 0){

                    _timerAttack += timeAttack1;
                    speedMovement = speedMovement/4;
                    playerControlJump.jumpActive = false;
                    batWeapon.damage = 1;
                }

                else if (_attackHit == 1){

                    _timerAttack += timeAttack2;
                    batWeapon.damage = 1;
                }

                else if (_attackHit == 2){

                    _timerAttack += timeAttack3;
                    batWeapon.damage = 3;
                }

                _attackHit++;
            }

            if (_attackHit > 0){

                if (_timerAttack > 0){

                    _timerAttack -= Time.deltaTime;
                }

                else{

                    _animator.SetBool("Attack", false);
                    playerControlJump.jumpActive = true;
                    speedMovement = _normalSpeedMovement;
                    _attackHit = 0;           
                }
            }
        }

        if (_landingToIdleActive){
            
            if (timeLandingToIdle > 0){

                timeLandingToIdle -= Time.deltaTime;
            }

            if (timeLandingToIdle <= 0){

                _animator.SetBool("Idle", true);
                _animator.SetBool("Landing", false);
                timeLandingToIdle = _timeAuxiliar;
                _landingToIdleActive = false;
            }
        }

        if (_bootsActivate == true){

            if (Input.GetKeyDown(KeyCode.RightShift) && _exhausted == false && !_animator.GetBool("Jumping") && !_animator.GetBool("Falling")){

                speedMovement = 2 * _normalSpeedMovement;
                _wasteEnergy = true;
                _rechargeEnergy = false;
                spendingEnergy.Play();
            }

            if (Input.GetKeyUp(KeyCode.RightShift) && _exhausted == false){

                speedMovement = _normalSpeedMovement;   
                _wasteEnergy = false;
                _rechargeEnergy = true;     
                spendingEnergy.Stop();        
            }

            if (_wasteEnergy && !_animator.GetBool("Jumping") && !_animator.GetBool("Falling")){

                WastingEnergy();
            }

            if (_rechargeEnergy){

                RechargingEnergy();
            }
        }

        if (_hurtVoiceActivate == false && hurtVoices[_randomIndex].isPlaying == false){

            _hurtVoiceActivate = true;
        }

        Debug.DrawRay(floorCollider.position - new Vector3(0, 0.075f, 0), Vector3.down * fallMaxDistance, Color.blue);
        Debug.DrawRay(ceilingCollider.position, Vector3.up * minDistanceToJump, Color.red);
        Debug.DrawRay(floorCollider.position, _surfaceNormal, Color.yellow);
    }

    public void StopMovement(){

        playerControlMovement.StopControlMovement();
    }

    public void TakingDamage(float damage){

        if (_activeTrigger){

            health -= damage;
            particleBlood.Play();

            if (health <= 0){

                greenHealthBar.fillAmount = 0;
                GetComponent<Collider>().enabled = false;
                _levelManager.StartCoroutine(_levelManager.InDeathByBlow());
            }

            else{

                greenHealthBar.fillAmount = health/_maxHealth;

                if (_hurtVoiceActivate){

                    _randomIndex = _random.Next(0, hurtVoices.Count);
                    hurtVoices[_randomIndex].Play();
                    _hurtVoiceActivate = false;
                }
            }
        }
    }

    public void Healing(int healthPoints, Cure cure){

        if (health < 10){

            var healingSound = cure.healingSound;
            healingSound.PlayOneShot(healingSound.clip);
            particleHealth.Play();

            if (health + healthPoints >= 10){

                health = 10;
                greenHealthBar.fillAmount = _maxHealth;
            }

            else{

                health += healthPoints;
                greenHealthBar.fillAmount = health/_maxHealth;
            }

            Destroy(cure.gameObject);
        }
    }

    public void Life(int lifePoints, Life life){

        var healingSound = life.lifeSound;
        healingSound.PlayOneShot(healingSound.clip);
        particleHealth.Play();
        lifeCounter.text = (Convert.ToInt32(lifeCounter.text) + lifePoints).ToString();
        Destroy(life.gameObject);      
    }

    public void ColliderWeaponActive(){

        weapon.enabled = true;
    }

    public void ColliderWeaponDeactive(){

        weapon.enabled = false;
    }

    void WastingEnergy(){

        if (yellowEnergyBar.fillAmount > 0){

            yellowEnergyBar.fillAmount -= energyDrainPerSecond * Time.deltaTime;
        }

        else{

            yellowEnergyBar.fillAmount = 0;
            yellowEnergyBar.color = new Color(1f, 0, 0);
            speedMovement = _normalSpeedMovement; 
            _exhausted = true;
            _rechargeEnergy = true;
            _wasteEnergy = false;
            spendingEnergy.Stop();
            emptyEnergy.Play();
        }
    }

    void RechargingEnergy(){

        if (yellowEnergyBar.fillAmount < 1f){

            if (_exhausted){

                yellowEnergyBar.fillAmount += energyRechargePerSecond * 2 * Time.deltaTime;
            }

            else{

                yellowEnergyBar.fillAmount += energyRechargePerSecond * Time.deltaTime;
            }
        }

        else{

            yellowEnergyBar.fillAmount = 1f;
            yellowEnergyBar.color = new Color(1f, 1f, 1f);
            _exhausted = false;
            _rechargeEnergy = false;
            emptyEnergy.Stop();
        }        
    }
}
