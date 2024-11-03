using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    [SerializeField] private float changeDuration = 3f;
    private Material material;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        Color startColor = Color.white;
        Color endColor = Color.white;

        ColorUtility.TryParseHtmlString("#FFF000", out startColor);
        ColorUtility.TryParseHtmlString("#797424", out endColor);

        while (true)
        {
            float timeElapsed = 0f;
            while (timeElapsed < changeDuration)
            {
                material.color = Color.Lerp(startColor, endColor, timeElapsed / changeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
    }
}
