using UnityEngine;

public class GhostChase : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public SpriteRenderer sr;

    public ParticleSystem auraBlack; // 검은 일렁임 파티클
    public ParticleSystem auraBlue;  // 파란 일렁임 파티클
    public ParticleSystem ghostSmoke; // 푸른 연기 파티클

    [Header("등장 및 추적 조건")]
    public float appearHeight = 28f;
    public float chaseMinY = 24f;
    public float moveSpeed = 5f;

    [Header("페이드아웃 조건")]
    public float fadeOutSpeed = 0.5f;             // 스프라이트 알파 감소 속도
    public float smokeDelay = 0.7f;               // FadeOut 시작 후 파티클 등장 지연 시간 (초 단위)
    public float requiredHeightToEscape = 43.0f;

    [HideInInspector] public bool safeZoneReached = false;

    private Rigidbody2D rb;
    private bool hasAppeared = false;
    private bool smokePlayed = false;    // 파티클 한 번만 실행 플래그
    private float fadeTimer = 0f;        // 페이드아웃 경과 시간 측정용
    private Color originalColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (sr != null)
            originalColor = sr.color;
    }

    void Start()
    {
        // 유령 스프라이트 비활성화
        if (sr != null)
        {
            sr.enabled = false;
            sr.color = originalColor;
        }

        // 모든 파티클 Stop 상태
        if (auraBlack != null) auraBlack.Stop();
        if (auraBlue != null) auraBlue.Stop();
        if (ghostSmoke != null) ghostSmoke.Stop();

        rb.linearVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        // 1) 유령 등장 처리
        if (!hasAppeared && player.position.y > appearHeight)
        {
            hasAppeared = true;

            // 스프라이트 켜기
            if (sr != null)
            {
                sr.enabled = true;
                sr.color = originalColor;
            }

            // 일렁임 파티클 재생
            if (auraBlack != null) auraBlack.Play();
            if (auraBlue != null) auraBlue.Play();

            Debug.Log("👻 유령 등장! (스프라이트 + aura 파티클 재생)");
        }

        if (!hasAppeared)
            return;

        // 2) SafeZone 진입 시 페이드아웃 로직 진입
        if (safeZoneReached)
        {
            FadeOutAndStop();
            return;
        }

        // 3) 추적 로직
        if (player.position.y < chaseMinY)
            return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        Vector2 targetPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    void FadeOutAndStop()
    {
        if (sr == null) return;

        // 페이드아웃 경과 시간 누적
        fadeTimer += Time.fixedDeltaTime;

        // ① FadeOut이 시작된 첫 프레임(= safeZoneReached가 처음 true가 된 순간)엔 이 지점부터 fadeTimer가 누적됩니다.
        // ② fadeTimer >= smokeDelay가 된 순간, 한 번만 smoke 파티클 재생
        if (!smokePlayed && fadeTimer >= smokeDelay)
        {
            smokePlayed = true;

            // aura 파티클 즉시 멈추기
            if (auraBlack != null) auraBlack.Stop();
            if (auraBlue != null) auraBlue.Stop();

            // 푸른 연기 파티클 재생
            if (ghostSmoke != null)
            {
                if (!ghostSmoke.gameObject.activeInHierarchy)
                    ghostSmoke.gameObject.SetActive(true);

                ghostSmoke.transform.SetParent(null);
                ghostSmoke.Play();

                // 파티클 지속 시간 뒤에 삭제 (필요 시 더 짧게 조정 가능)
                float totalLifetime = ghostSmoke.main.duration
                                    + ghostSmoke.main.startLifetime.constantMax;
                Destroy(ghostSmoke.gameObject, totalLifetime);
            }

            Debug.Log("🌫 Smoke 파티클 재생 (페이드아웃 " + smokeDelay + "초 후)");
        }

        // ▶ 스프라이트 알파 감소 (항상 진행)
        Color c = sr.color;
        c.a -= fadeOutSpeed * Time.fixedDeltaTime;
        sr.color = c;

        rb.linearVelocity = Vector2.zero;
        Debug.Log("🌫 페이드아웃 중 → alpha: " + c.a);

        // ▶ 알파가 0 이하가 되면 스프라이트·스크립트 비활성화
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
                fadeTimer = 0f;         // ▶ FadeOut 시작 시 타이머 초기화
                smokePlayed = false;    // ▶ 재생 플래그 초기화 (혹시 재진입할 경우 대비)
                Debug.Log("🟢 높이 충족 → 유령 사라짐 준비 (FadeOut 시작)");
            }
            else
            {
                Debug.Log("⚠ 높이 미달 → 사라지지 않음");
            }
        }
    }
}
