using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class ScoreDisplay : MonoBehaviour
{
    private Text textComponent;

    private void Awake() {
        textComponent = GetComponent<Text>();
    }
    private void Update() {
        if (Time.frameCount % 3 == 0)
            textComponent.text = ((int)CharacterController2D.current.score).ToString();
    }
}
