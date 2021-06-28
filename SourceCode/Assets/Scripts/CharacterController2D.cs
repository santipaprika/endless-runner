using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class CharacterController2D : MonoBehaviour {
    public static CharacterController2D current;
    [HideInInspector] public bool gameOver = false;

    [SerializeField] private float m_JumpForce = 400f;
    [SerializeField] public float m_CharacterSpeed = 1f;

    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    [HideInInspector] public bool jump = false;

    [HideInInspector] public float score;
    [SerializeField] private float scorePerSec = 50;

    [HideInInspector] public bool immune = false;
    [HideInInspector] public float immuneTimer = 0.0f;
    [HideInInspector] public Material originalMaterial;

    private void Awake() {
        current = this;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        score = 0;
        float window_width = Camera.main.orthographicSize * Screen.width / Screen.height;
        transform.position = new Vector3(-window_width + window_width / 10f, 0, 0);
        originalMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update() {
        GetComponent<Animator>().SetBool("Jump", jump);

        if (Input.GetButtonDown("Jump") && !jump) {
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            SoundManager.current.PlaySound("Jump");
        }

        jump = true;

        if (!gameOver) {
            score += Time.deltaTime * scorePerSec;
            if (immune) {
                if (immuneTimer >= LevelGenerator.current.immuneDuration) {
                    immune = false;
                    GetComponent<Renderer>().material = originalMaterial;
                } else
                    immuneTimer += Time.deltaTime;
            }
        }
    }

    private void LateUpdate() {
        if (transform.position.y < -Camera.main.orthographicSize)
            GameOver();
    }

    private void GameOver() {
        if (gameOver) return;   //Do not repeat this function more than once

        SoundManager.current.PlaySound("Fire");
        foreach (LevelMovement gameElement in FindObjectsOfType<LevelMovement>()) {
            gameElement.enabled = false;
        }
        LevelGenerator.gameOverObject.SetActive(true);
        gameOver = true;

        if (score > GlobalScore.current.maxScore)
            GlobalScore.current.maxScore = (int)score;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy" && !immune)
            GameOver();
        else if (collision.gameObject.tag == "PowerUp") {
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/SpriteOutline");
            immune = true;
            immuneTimer = 0f;
            Destroy(collision.gameObject);
        }
    }
}
