using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public Transform ball;
    public float ballPeriod = 3.0f;
    private float counter = 0.0f;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= ballPeriod) {
            Instantiate(ball, transform);
            counter = 0.0f;
        }
    }
}
