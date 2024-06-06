using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public bool appear;
    public bool fade;

    public float time;

    GameManager _gameManager;
    float _alpha;

    void Start(){

        _gameManager = GameManager.instance;

        if (appear){

            _alpha = 0;
        }

        if (fade){

            _alpha = 1f;
        }
    }

    void Update(){

        if (appear){

            Appear();
        }

        if (fade){

            Fade();
        }

        GetComponent<Image>().color = new Color(0,0,0, _alpha);
    }

    void Appear(){

        if (_alpha < 1f){

            _alpha += 1f * Time.deltaTime * 1/time;
        }

        else {

            _alpha = 1f;

            if (_gameManager.OnBlackScreen != null){

                _gameManager.OnBlackScreen.Invoke();
                _gameManager.OnBlackScreen = null;
            }

            appear = false;
        }
    }

    void Fade(){

        if (_alpha > 0){

            _alpha -= 1f * Time.deltaTime * 1/time;
        }

        else{

            _alpha = 0;

            if (_gameManager.OffBlackScreen != null){

                _gameManager.OffBlackScreen.Invoke();
                _gameManager.OffBlackScreen = null;
            }
            
            fade = false;
        }
    }
}
