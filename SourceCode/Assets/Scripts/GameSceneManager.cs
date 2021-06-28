using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class GameSceneManager : MonoBehaviour {

    public void changeScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void exit() {
        Application.Quit();
    }
}
