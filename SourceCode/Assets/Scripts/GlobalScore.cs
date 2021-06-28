using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class GlobalScore : MonoBehaviour
{
    public static GlobalScore current;

    [HideInInspector] public int maxScore;

    private void Awake() {
        if (!current) {
            DontDestroyOnLoad(gameObject);
            current = this;
            maxScore = 0;
        } else if (current != this) {
            Destroy(gameObject);
        }
    }
}
