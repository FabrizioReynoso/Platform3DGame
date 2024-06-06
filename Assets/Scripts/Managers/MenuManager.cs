using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Credits;

    [SerializeField] AudioSource soundButtonPlay;
    //[SerializeField] AudioSource soundButtonClicked;

    public void Play(){

        soundButtonPlay.Play();
        SceneManager.LoadScene(1);
    }

    public void OnCredits(){

        Credits.SetActive(true);
        //soundButtonClicked.Play();
    }

    public void Back(){

        Credits.SetActive(false);
    }

    public void OnQuit(){

        Application.Quit();
        //soundButtonClicked.Play();
    }
}
