using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] GyroDropUIManager _gyrodropUIManager;

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
        pieceCollider.enabled = false;

        for (int i = 0; i < blinkCount; i++)
        {
            pieceRenderer.enabled = !pieceRenderer.enabled; // πﬂ∆« ±Ù∫˝¿”
            yield return new WaitForSeconds(blinkDuration);
        }

        // ±Ù∫˝¿” »ƒ πﬂ∆«¿ª ∫Ò»∞º∫»≠
        pieceRenderer.enabled = false;
        pieceCollider.enabled = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && pieceRenderer.enabled == false ) 
        {
            FindObjectOfType<GyroDropGameManager>().HandleCollision();

            _gyrodropUIManager.StartWarning(); // «œ∞≠«œ∏Èº≠ Finish Ω««‡«ÿ¡‹ 
        }
    }
}
