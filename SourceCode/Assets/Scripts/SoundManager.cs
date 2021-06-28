using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;

    public AudioClip JumpSound;
    public AudioClip FireSound;
    private AudioSource audioSource;

    private void Awake() {
        current = this;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string clip) {
        switch (clip) {
            case "Jump":
                audioSource.PlayOneShot(JumpSound);
                break;
            case "Fire":
                audioSource.PlayOneShot(FireSound);
                break;
        }
    }
}
