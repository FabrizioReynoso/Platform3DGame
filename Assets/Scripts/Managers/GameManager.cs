using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;
    public BlackScreen blackScreen;
    public Action OnBlackScreen;
    public Action OffBlackScreen;

    void Awake(){

        instance = this;
    }

    public void BlackScreenAppear(){

        blackScreen.appear = true;
    }

    public void BlackScreenFade(){

        blackScreen.fade = true;
    }
}
