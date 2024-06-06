using UnityEngine;

public class PlatformTrapFall : PlatformTrap
{
    [SerializeField] AudioSource groundHitSound;
    [SerializeField] Material MaterialFade;
    [SerializeField] float _fadingTime;

    AudioSource _audioSource;
    MeshRenderer _meshRenderer;
    Color _color;
    float _alpha;
    bool _OnFading;

    public override void Start()
    {
        base.Start();
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _color = _meshRenderer.material.color;
        _alpha = _meshRenderer.material.color.a;
    }

    public override void OnTriggerEnter(Collider other){

        base.OnTriggerEnter(other);

        var checkFloor = other.GetComponent<CheckFloor>();
        var checkCeiling = other.GetComponent<CheckCeiling>();

        if (trapActive){

            if (checkFloor){

                Falling();
            }

            if (!checkFloor && !checkCeiling){

                Landing();
            }
        }
    }

    void OnCollisionEnter(Collision other){

        var player = other.gameObject.GetComponent<Player>();

        if (trapActive && !player){

            Landing();
        }
    }

    void Update(){

        if (_OnFading){

            if (_alpha > 0){

                _alpha -= 1/_fadingTime * Time.deltaTime;
                _meshRenderer.material.color = new Color(_color.r, _color.g, _color.b, _alpha);
            }

            else{

                m_player.IsFloor = false;
                m_player.transform.SetParent(null);
                Destroy(gameObject);
            }
        }

        if (_audioSource.isPlaying){

            if (_audioSource.volume > 0){

                _audioSource.volume -= 1/5f * Time.deltaTime;
            }

            else{

                _audioSource.Stop();
                _audioSource.volume = 1f;
            }
        }
    }

    void Falling(){

        _audioSource.Play();
        ActivatingTrap();
        Destroy(gameObject, 10f);
    }

    void Landing(){

        _audioSource.Stop();
        GetComponent<Rigidbody>().isKinematic = true;
        m_move = false;
        trapActive = false;
        _meshRenderer.material = MaterialFade;
        _OnFading = true;
        groundHitSound.PlayOneShot(groundHitSound.clip);    
    }
}
