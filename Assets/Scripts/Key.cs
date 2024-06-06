using UnityEngine;
using TMPro;

public class Key : MonoBehaviour
{
    public TextMeshProUGUI CountKey;
    
    [SerializeField] AudioSource _soundKeyTake;

    void OnTriggerEnter(Collider other){

        var player = other.GetComponent<Player>();

        if (player){

            _soundKeyTake.Play();
            CountKey.text = (System.Convert.ToInt32(CountKey.text) + 1).ToString();    
            LevelManager.instance.CheckPoint(player.transform.position);
            Destroy(gameObject);
        }
    }
}
