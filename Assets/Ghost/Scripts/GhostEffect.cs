using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector3 startPos;

    [Header("부유 효과 (위아래 흔들림)")]
    public float floatSpeed = 1.0f;
    public float floatHeight = 0.1f;

    [Header("깜빡임 효과 (투명도 변화)")]
    public float flickerSpeed = 1.5f;
    public float alphaMin = 0.25f;
    public float alphaMax = 0.45f;

    private GhostChase ghostChase;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;

        ghostChase = GetComponentInParent<GhostChase>();
    }

    void Update()
    {
        // ✅ 부유 효과는 계속
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0f, offsetY, 0f);

        // ✅ 깜빡임 효과는 사라지는 중이면 중단
        if (ghostChase != null && ghostChase.safeZoneReached)
            return;

        float alpha = Mathf.Lerp(alphaMin, alphaMax, (Mathf.Sin(Time.time * flickerSpeed) + 1f) / 2f);
        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, alpha);
    }
}
