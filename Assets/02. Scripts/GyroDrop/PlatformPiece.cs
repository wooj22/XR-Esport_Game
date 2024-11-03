using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] GyroDropUIManager _gyrodropUIManager;
    [SerializeField] GameObject warningUI; 

    private Renderer pieceRenderer;
    private Collider pieceCollider;

    private void Start()
    {
        pieceRenderer = GetComponent<Renderer>();
        pieceCollider = GetComponent<Collider>();

        if (warningUI != null)
        {
            warningUI.SetActive(false); // 시작 시 UI 비활성화
        }
    }

    public void StartBlinking(float blinkDuration, int blinkCount)
    {
        if (warningUI != null)
        {
            warningUI.SetActive(true); // 깜빡임 시작 시 UI 활성화
        }
        StartCoroutine(BlinkRoutine(blinkDuration, blinkCount));
    }

    private IEnumerator BlinkRoutine(float blinkDuration, int blinkCount)
    {
        pieceCollider.enabled = false;

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
        if (other.CompareTag("Player") && pieceRenderer.enabled == false ) 
        {
            FindObjectOfType<GyroDropGameManager>().HandleCollision();

            _gyrodropUIManager.StartWarning(); // 하강하면서 Finish 실행해줌 
        }
    }

    public void DeactivateWarningUI()
    {
        if (warningUI != null)
        {
            warningUI.SetActive(false); // UI 비활성화
        }
    }
}
