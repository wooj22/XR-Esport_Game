using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove1 : MonoBehaviour
{
    public float speed = 50f;          // 전진하는 속도
    public float waveFrequency = 2f;  // 좌우로 구불구불하는 빈도
    public float waveAmplitude = 0.05f;  // 좌우로 흔들리는 크기

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 전진하는 방향으로 이동 (Z축 기준)
        Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;

        // 구불구불하게 이동하는 X축 방향 (Sine 함수를 사용하여 좌우로 흔들림)
        float wave = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        Vector3 wavyMovement = new Vector3(wave, 0, 0);

        // 구불구불한 움직임과 전진을 합쳐서 최종 이동 처리
        transform.position += forwardMovement + wavyMovement;
    }
}
