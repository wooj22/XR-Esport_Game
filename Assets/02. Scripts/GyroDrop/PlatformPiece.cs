using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    private Renderer pieceRenderer;
    private Collider pieceCollider;

    private void Start()
    {
        pieceRenderer = GetComponent<Renderer>();
        pieceCollider = GetComponent<Collider>();
    }

    public void StartBlinking(float blinkDuration, int blinkCount)
    {
        StartCoroutine(BlinkRoutine(blinkDuration, blinkCount));
    }

    private IEnumerator BlinkRoutine(float blinkDuration, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            pieceRenderer.enabled = !pieceRenderer.enabled; // 발판 깜빡임
            yield return new WaitForSeconds(blinkDuration);
        }

        // 깜빡임 후 발판을 비활성화
        pieceRenderer.enabled = false;
        pieceCollider.enabled = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && pieceRenderer.enabled == false) // 투명할 때만 충돌 처리
        {
            // 카메라 하강 처리
            // FindObjectOfType<GyroDropGameManager>().LowerHeight();
        }
    }
}
