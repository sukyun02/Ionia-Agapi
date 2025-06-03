using UnityEngine;

public class GhostChase : MonoBehaviour
{
    [Header("참조")]
    public Transform player;                     // 추적 대상
    public SpriteRenderer sr;                    // 인스펙터에서 자식(GhostVisual)의 SpriteRenderer 직접 연결

    [Header("등장 및 추적 조건")]
    public float appearHeight = 28f;             // 유령 등장 Y 조건
    public float chaseMinY = 24f;                // 너무 내려가면 추적 중단
    public float moveSpeed = 5f;                 // 추적 속도

    [Header("페이드아웃 조건")]
    public float fadeOutSpeed = 0.5f;            // 페이드아웃 속도
    public float requiredHeightToEscape = 43.0f; // SafeZone에서 사라지기 위한 최소 Y

    [HideInInspector] public bool safeZoneReached = false;

    private Rigidbody2D rb;
    private bool hasAppeared = false;
    private Color originalColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 인스펙터에서 sr을 직접 연결해야 하므로 자동 할당 제거
        if (sr != null)
            originalColor = sr.color;
    }

    void Start()
    {
        if (sr != null)
        {
            sr.enabled = false;
            sr.color = originalColor;
        }

        rb.linearVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (!hasAppeared && player.position.y > appearHeight)
        {
            hasAppeared = true;

            if (sr != null)
            {
                sr.enabled = true;
                sr.color = originalColor;
            }

            Debug.Log("👻 유령 등장!");
        }

        if (!hasAppeared)
            return;

        if (safeZoneReached)
        {
            FadeOutAndStop();
            return;
        }

        if (player.position.y < chaseMinY)
            return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        Vector2 targetPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    void FadeOutAndStop()
    {
        if (sr == null) return;

        Color c = sr.color;
        c.a -= fadeOutSpeed * Time.fixedDeltaTime;
        sr.color = c;

        rb.linearVelocity = Vector2.zero;

        Debug.Log("🌫️ 페이드아웃 중 → alpha: " + c.a);

        if (c.a <= 0f)
        {
            sr.enabled = false;
            this.enabled = false;
            Debug.Log("💀 유령 완전 소멸");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SafeZone"))
        {
            Debug.Log("🎯 SafeZone 감지됨, playerY = " + player.position.y);

            if (player.position.y >= requiredHeightToEscape)
            {
                safeZoneReached = true;
                Debug.Log("🟢 높이 충족 → 유령 사라짐 시작");
            }
            else
            {
                Debug.Log("⚠️ 높이 미달 → 사라지지 않음");
            }
        }
    }
}
