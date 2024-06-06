using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    LevelManager _levelManager;
    Player _player;

    void Start(){

        _levelManager = LevelManager.instance;
        _player = Player.instance;
    }

    public void Continue(){

        Time.timeScale = 1f;
        _player.ControlsActive = true;
        gameObject.SetActive(false);
        _levelManager.HUDActive();
    }

    public void RestartLevel(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){

        SceneManager.LoadScene(0);
    }
}
