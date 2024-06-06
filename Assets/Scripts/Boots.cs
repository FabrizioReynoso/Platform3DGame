using UnityEngine;

public class Boots : MonoBehaviour
{
    [SerializeField] GameObject bootsPoster;
    [SerializeField] GameObject energyBar;

    void OnTriggerEnter(Collider other){

        var player = other.GetComponent<Player>();

        if (player){

            bootsPoster.SetActive(true);
            energyBar.SetActive(true);
            player.BootsActivate = true;
            player.StopMovement();
            player.ControlsActive = false;
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }
}
