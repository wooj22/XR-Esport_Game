using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove2 : MonoBehaviour
{
    public float speed = 50f;          // 전진하는 속도


    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 전진하는 방향으로 이동 (Z축 기준)
        Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;
        transform.position += forwardMovement;
    }
}
