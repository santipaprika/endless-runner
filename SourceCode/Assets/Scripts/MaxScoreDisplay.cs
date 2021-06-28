using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class MaxScoreDisplay : MonoBehaviour
{
    private Text textComponent;

    private void Start() {
        GetComponent<Text>().text = GlobalScore.current.maxScore.ToString();
    }
}
