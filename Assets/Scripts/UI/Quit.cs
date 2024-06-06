using UnityEngine;

public class Quit : MonoBehaviour
{
    [SerializeField] Player player;

    [Header("Optional")]
    [SerializeField] GameObject panelActive;

    void Update(){
        
        if (Input.GetKeyDown(KeyCode.Q)){

            gameObject.SetActive(false);
            player.ControlsActive = true;
            Time.timeScale = 1f;

            if (panelActive){

                panelActive.SetActive(true);
            }
        }
    }
}
