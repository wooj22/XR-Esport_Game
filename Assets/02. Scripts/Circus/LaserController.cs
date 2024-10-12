using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private Material redMaterial;
    private float laserSpeed;
    private bool isLaserTouchingPlayer = false;
    private CircusGameManager _circusGameManager;

    private void Start()
    {
        _circusGameManager = GameObject.Find("GameManager").GetComponent<CircusGameManager>();
        laserSpeed = _circusGameManager.currentLaserSpeed;
    }

    private void Update()
    {
        // 전방향 이동
        if (!isLaserTouchingPlayer)
        {
            float moveDir = laserSpeed * Time.deltaTime;
            transform.Translate(0, 0, moveDir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // DownBorder와 충돌
        if(other.gameObject.tag == "LaserDownBorder")
        {
            _circusGameManager.OnLaserReachBorder();
            Destroy(this.gameObject);
        }
        // Player와 충돌
        else if (other.gameObject.tag == "Player")
        {
            _circusGameManager.OnLaserHitPlayer();
            isLaserTouchingPlayer = true;

            // 빨간 머테리얼로 교체
            foreach (Transform child in transform)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                meshRenderer.material = redMaterial;
            }
            Destroy(this.gameObject, 3f);
        }
    }
}
