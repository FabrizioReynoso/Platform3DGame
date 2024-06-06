using UnityEngine;
using TMPro;

public class Relic : MonoBehaviour
{
    [SerializeField] AudioSource getRelicSound;
    [SerializeField] TextMeshProUGUI _textRelicCount;

    Collider _collider;
    Color _color;
    Material _material;

    void Start(){

        _collider = GetComponent<Collider>();
        _material = GetComponent<Renderer>().material;
        _color = _material.color;
    }

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Player>()){

            getRelicSound.Play();
            Destroy(gameObject);
        }
    }

    void OnDestroy(){

        _textRelicCount.text = (System.Convert.ToInt32(_textRelicCount.text) + 1).ToString();
    }

    public void Active(){

        _collider.enabled = true;
        _material.color = new Color(_color[0], _color[1], _color[2], 1f);
    }
}
