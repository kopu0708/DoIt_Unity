using System.Collections;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(FadeLoop));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(FadeLoop));
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return FadeEffect.Fade(spriteRenderer, 1, 0, 0.5f);
            yield return FadeEffect.Fade(spriteRenderer, 0, 1, 0.5f);
        }
    }
}
