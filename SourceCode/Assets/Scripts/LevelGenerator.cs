using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class LevelGenerator : MonoBehaviour
{
    [HideInInspector]
    public static GameObject gameOverObject;
    public static LevelGenerator current;

    [SerializeField] private Transform[] platforms = null;
    [SerializeField] private bool[] canGenerateEnemies = null; //choose in which platforms we will try to generate enemies

    [SerializeField] private float space = 2;
    [SerializeField] private float spaceVariance = 0.5f;
    [SerializeField] private float heightVariance = 7;

    [SerializeField] private Transform immunePowerUp = null;
    [SerializeField] private float PowerUpProbability = 0.05f;
    [SerializeField] public float immuneDuration = 10f;
    [SerializeField] private Transform[] enemies = null;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float[] enemiesProbability = null;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float enemiyVarianceFromPlatformCenter = 0.7f;    //can appear in -> 1: whole platform - ... - 0: only center

    private Transform lastPlatform_level1;
    
    private float screenWorldWidth;
    private float screenWorldHeight;
    private float platformHeight;
    private float platformMaxWidth;

    private Vector3 endPointSecPlatform;
    private float[] cumulativeEnemiesProbability = new float[2];

    private void Awake() {
        current = this;
        gameOverObject = GameObject.FindGameObjectWithTag("Game Over");
        Debug.Assert(gameOverObject, "Game Over object (with 'Game Over' tag) not found");
        if (!gameOverObject) { Debug.Break(); return; }
        gameOverObject.SetActive(false);
    }

    void Start() {
        screenWorldHeight = Camera.main.orthographicSize;
        screenWorldWidth = screenWorldHeight * Screen.width / Screen.height;

        GenerateFirstPlatforms();

        cumulativeEnemiesProbability[0] = enemiesProbability[0];
        for (int enemyIdx = 1; enemyIdx < enemiesProbability.Length; enemyIdx++) {
            cumulativeEnemiesProbability[enemyIdx] = cumulativeEnemiesProbability[enemyIdx - 1] + enemiesProbability[enemyIdx];
        }
    }

    void GenerateFirstPlatforms() {
        lastPlatform_level1 = null;
        lastPlatform_level1 = Instantiate(platforms[0], new Vector3(-screenWorldWidth + screenWorldWidth/10f, -5f), Quaternion.identity);

        platformHeight = lastPlatform_level1.GetComponent<BoxCollider2D>().bounds.size.y;   //Same for all
        platformMaxWidth = lastPlatform_level1.GetComponent<BoxCollider2D>().bounds.size.x;   //Same for all

        lastPlatform_level1 = InstantiatePlatform(lastPlatform_level1.Find("EndPosition").position);
        lastPlatform_level1 = InstantiatePlatform(lastPlatform_level1.Find("EndPosition").position);
        lastPlatform_level1 = InstantiatePlatform(lastPlatform_level1.Find("EndPosition").position);
    }

    void Update()
    {
        lastPlatform_level1 = CheckAndSetNewPlatform(lastPlatform_level1);
    }

    private Transform InstantiatePlatform(Vector3 position) {
        int platformIdx = Random.Range(0, platforms.Length);
        Transform newPlatform = Instantiate(platforms[platformIdx], position, Quaternion.identity);

        float currentPlatformWidth = newPlatform.GetComponent<Collider2D>().bounds.size.x;

        //Instantiate powerup (maybe)
        if (Random.value < PowerUpProbability) {
            float powerUpXPosition = currentPlatformWidth / 2.0f + Random.Range(-currentPlatformWidth / 2.0f, currentPlatformWidth / 2.0f);
            Transform newPowerUp = Instantiate(immunePowerUp, position + new Vector3(powerUpXPosition, platformHeight, 0), Quaternion.identity);
            newPowerUp.position += new Vector3(0, newPowerUp.GetComponent<Renderer>().bounds.extents.y, 0);
        }

        //Instantiate enemy (maybe)
        int enemySelectedIdx = -1;
        float random = Random.value;
        for (int enemyIdx = 0; enemyIdx < enemiesProbability.Length; enemyIdx++) {
            if (random < cumulativeEnemiesProbability[enemyIdx]) {
                enemySelectedIdx = enemyIdx;
                break;
            }
        }

        if (canGenerateEnemies[platformIdx] && enemySelectedIdx != -1) {
            float enemyXPosition = currentPlatformWidth / 2.0f + Random.Range(-currentPlatformWidth / 2.0f, currentPlatformWidth / 2.0f) * enemiyVarianceFromPlatformCenter;
            Transform newEnemy = Instantiate(enemies[enemySelectedIdx], position + new Vector3(enemyXPosition, platformHeight, 0), Quaternion.identity);
            newEnemy.position += new Vector3(0, newEnemy.GetComponent<Renderer>().bounds.extents.y, 0);
        }
        
        return newPlatform;
    }

    private bool OutOfScreen(float platformHeight) {
        return (platformHeight <= -screenWorldHeight || platformHeight >= screenWorldHeight - platformHeight * 2);
    }

    Transform CheckAndSetNewPlatform(Transform lastPlatform) {
        if (lastPlatform.position.x <= screenWorldWidth) {

            Vector3 endPosition = lastPlatform.Find("EndPosition").position;

            float spaceAdded = space + Random.Range(-spaceVariance / 2.0f, spaceVariance / 2.0f);
            float heightAdded = Random.Range(-heightVariance / 2.0f, heightVariance / 2.0f);

            if (OutOfScreen(endPosition.y + heightAdded)) {
                heightAdded -= heightAdded * 2;
            }

            Vector3 newPosition = endPosition + new Vector3(spaceAdded, heightAdded, 0);
            Transform newPlatform = InstantiatePlatform(newPosition);

            return newPlatform;
        }
        
        return lastPlatform;
    }

    public void ResetLevel() {
        LevelGenerator.current.lastPlatform_level1 = null;
        CharacterController2D character = CharacterController2D.current;
        foreach (LevelMovement gameElement in FindObjectsOfType<LevelMovement>()) {
            DestroyImmediate(gameElement.gameObject);
        }
        current.GenerateFirstPlatforms();
        float window_width = Camera.main.orthographicSize * Screen.width / Screen.height;
        character.transform.position = new Vector3(-window_width + window_width / 10f, 0, 0);
        character.m_Rigidbody2D.velocity = new Vector3(0, 0, 0);
        gameOverObject.SetActive(false);
        character.gameOver = false;
        character.score = 0.0f;
        character.GetComponent<Renderer>().material = character.originalMaterial;
        character.immune = false;
        character.immuneTimer = 0.0f;
    }
}
