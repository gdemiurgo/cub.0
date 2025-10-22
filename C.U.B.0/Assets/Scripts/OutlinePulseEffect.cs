using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class OutlinePulseEffect : MonoBehaviour
{
    public float minX = -1f;
    public float maxX = 1f;
    public float minY = -1f;
    public float maxY = 1f;

    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;

    public float pulseSpeed = 1.5f; // velocidad del cambio

    private Outline outline;
    private Color baseColor;
    private Vector2 baseEffectDistance;

    void Start()
    {
        outline = GetComponent<Outline>();
        baseColor = outline.effectColor;
        baseEffectDistance = outline.effectDistance;

        StartCoroutine(GlitchRoutine());
    }

    void Update()
    {
        // Oscila suavemente con seno
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // 0–1

        float newX = Mathf.Lerp(minX, maxX, t);
        float newY = Mathf.Lerp(minY, maxY, 1f - t); // inverso, crea movimiento cruzado
        outline.effectDistance = new Vector2(newX, newY);

        // Oscila alpha entre min y max
        Color c = baseColor;
        c.a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * pulseSpeed * 0.5f, 1f));
        outline.effectColor = c;
    }

    IEnumerator GlitchRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.4f, 1.2f));

            // Pequeño salto brusco
            outline.effectDistance = new Vector2(
                Random.Range(minX * 2, maxX * 2),
                Random.Range(minY * 2, maxY * 2)
            );

            Color c = outline.effectColor;
            //c.a = Random.Range(minAlpha * 0.5f, maxAlpha);
            c.a = Random.Range(0.5f, 0.8f);
            outline.effectColor = c;

            yield return new WaitForSeconds(0.06f); // duración del glitch

            // Vuelve al valor base
            outline.effectDistance = baseEffectDistance;
            outline.effectColor = baseColor;
        }
    }
}
