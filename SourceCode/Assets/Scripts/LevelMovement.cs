using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Santi Paprika as part of the prototype project for alberms88 on Fiverr 

public class LevelMovement : MonoBehaviour
{
    public float speed = 1.0f;
    float screenLeftBorder;
    private Collider2D objectCollider2D;

    private void Start() {
        screenLeftBorder = Camera.main.orthographicSize * Screen.width / Screen.height;
        objectCollider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (transform.position.x + objectCollider2D.bounds.size.x < -screenLeftBorder)
            Destroy(gameObject);

        transform.position -= new Vector3(Time.deltaTime * CharacterController2D.current.m_CharacterSpeed * speed,0,0);
    }

    private void LateUpdate() {
        if (CharacterController2D.current.m_Rigidbody2D.IsTouching(objectCollider2D))
            CharacterController2D.current.jump = false;
    }
}
