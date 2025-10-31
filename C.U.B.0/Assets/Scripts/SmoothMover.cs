using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    public enum MoveMode
    {
        OneWay,
        Loop
    }

    [Header("References")]
    public Transform pointA;
    public Transform pointB;

    [Header("Settings")]
    public MoveMode moveMode = MoveMode.Loop;
    public float speed = 1f;
    public bool startOnPlay = true;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingForward = true;
    private bool isMoving = false;
    private float t = 0f;

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning($"{name}: Missing pointA or pointB reference.");
            enabled = false;
            return;
        }

        startPos = pointA.position;
        endPos = pointB.position;   

        if (startOnPlay)
            isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        // Mueve suavemente usando Lerp
        t += (movingForward ? 1 : -1) * speed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(startPos, endPos, t);

        // Llegó a un extremo
        if (t >= 1f || t <= 0f)
        {
            if (moveMode == MoveMode.Loop)
            {
                movingForward = !movingForward;
            }
            else // OneWay
            {
                isMoving = false;
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }
}
