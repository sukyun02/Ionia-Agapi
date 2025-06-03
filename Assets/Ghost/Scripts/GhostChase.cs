using UnityEngine;

public class GhostChase : MonoBehaviour
{
    public Transform player;
    public float minSpeed = 0.5f;         // 멀리 있을 때 속도
    public float maxSpeed = 3.0f;         // 가까울 때 속도
    public float detectionRange = 10f;    // 추적 시작 거리

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // 가까울수록 빨라짐 (거리 반비례)
            float speed = Mathf.Lerp(maxSpeed, minSpeed, distance / detectionRange);
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
