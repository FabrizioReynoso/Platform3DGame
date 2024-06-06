using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    int _indexScene;

    void Start(){

        _indexScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void Continue(){

        SceneManager.LoadScene(_indexScene+1);
    }

    public void Quit(){

        SceneManager.LoadScene(0);
    }
}
