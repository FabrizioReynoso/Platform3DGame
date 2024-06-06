using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public bool detection;
    public Animator animator;

    [SerializeField] Player player;
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource hitSound;
    [SerializeField] float movementSpeed;
    [SerializeField] float health = 5f;
    [SerializeField] float firstAttackDelay;
    [SerializeField] float attackRepeatRate;
    [SerializeField] float detectionRange;
    [SerializeField] float attackRange;
    [SerializeField] float deadTimer;

    Transform _transform;
    Transform _playerT;
    RaycastHit _hit1;
    RaycastHit _hit2;
    bool _moveTowardPlayer;
    bool _damaged;
    bool _detectingPlayer;
    bool _attackingPlayer;
    int _hitCounter;

    public bool Damaged{ get{ return _damaged; }}

    void Start(){

        _transform = transform;
        _playerT = player.transform;
        animator.SetBool("Idle", true);
    }

    void FixedUpdate(){

        if (_moveTowardPlayer){

            var moveDirection = new Vector3(_playerT.position.x - _transform.position.x, 0, _playerT.position.z - _transform.position.z).normalized;

            rb.MovePosition(_transform.position + moveDirection * movementSpeed * Time.fixedDeltaTime);
        }

        // Raycast para detectar a jugador

        if (Physics.Raycast(_transform.position, player.transform.position - _transform.position + new Vector3(0, 0.5f, 0), out _hit1, detectionRange)){

            if (_hit1.collider.GetComponent<Player>() && _detectingPlayer == false){

                animator.SetBool("Idle", false);
                MoveTowardPlayer(); 
                _detectingPlayer = true;
            }       
        }

        else if (_detectingPlayer){

            animator.SetBool("Idle", true);
            StopMovementTowardPlayer();  
            _detectingPlayer = false;              
        }

        // Raycast para atacar a jugador

        if (Physics.Raycast(_transform.position, player.transform.position - _transform.position + new Vector3(0, 0.5f, 0), out _hit2, attackRange)){

            if (_hit2.collider.GetComponent<Player>() && _attackingPlayer == false){

                InAttackPosition();
                _attackingPlayer = true;
            }
        }

        else if (_attackingPlayer){

            StopAttack();

            if (_detectingPlayer && _damaged == false){

                MoveTowardPlayer();
            }

            _attackingPlayer = false;              
        }

        Debug.DrawRay(transform.position, (player.transform.position - _transform.position + new Vector3(0, 0.5f, 0)).normalized * detectionRange, Color.yellow);
        Debug.DrawRay(transform.position, (player.transform.position - _transform.position + new Vector3(0, 0.5f, 0)).normalized * attackRange, Color.red);
    }

    void Update(){

        if (_moveTowardPlayer){

            var moveDirection = new Vector3(_playerT.position.x - _transform.position.x, 0, _playerT.position.z - _transform.position.z).normalized;

            _transform.forward = moveDirection;
        }
    }

    public void MoveTowardPlayer(){

        _moveTowardPlayer = true;
        animator.SetBool("Move", true);
    }

    public void InAttackPosition(){

        animator.SetBool("Idle", true);
        StopMovementTowardPlayer();
        InvokeRepeating("Attack", firstAttackDelay, attackRepeatRate);
    }

    public void StopMovementTowardPlayer(){

        _moveTowardPlayer = false;
        animator.SetBool("Move", false);   
    }

    public void StopAttack(){
  
        animator.SetBool("Attack", false);
        CancelInvoke("Attack"); 
    }

    public void ToDamage(int damage){

        hitSound.PlayOneShot(hitSound.clip);
        health -= damage;
        animator.SetBool("Idle", false);
        StopAttack();
        _damaged = true;

        if (health > 0){

            switch (_hitCounter){

                case 1:

                    animator.SetBool("LargeHit", true);
                    animator.SetBool("SmallHit", false);
                    _hitCounter = 0;    

                    break;

                default:

                    animator.SetBool("SmallHit", true);
                    animator.SetBool("LargeHit", false);
                    _hitCounter++;             

                    break;
            }
        }

        else{

            Die();          
        }
    }

    public void RecoverFromDamage(){

        _damaged = false;
        _hitCounter = 0;
        animator.SetBool("SmallHit", false);
        animator.SetBool("LargeHit", false);

        if (_attackingPlayer){

            InAttackPosition();
        } 

        else if (_detectingPlayer){

            MoveTowardPlayer();
        }

        else{

            StopMovementTowardPlayer();
            StopAttack();
        }
    }

    void Attack(){

        animator.SetBool("Attack", true);
        animator.SetBool("Idle", false);
        player.TakingDamage(1f);
        hitSound.PlayOneShot(hitSound.clip);
    }

    void Die(){

        animator.SetBool("Death", true);
        enabled = false;
        GetComponent<Collider>().enabled = false;
        StopMovementTowardPlayer();
        StopAttack();
        StartCoroutine(Dying());
        _damaged = false; 
    }

    IEnumerator Dying(){

        yield return new WaitForSeconds(deadTimer);

        Destroy(gameObject);
    }
}
