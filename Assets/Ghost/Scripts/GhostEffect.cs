using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector3 startPos;

    [Header("부유 효과 (위아래 흔들림)")]
    public float floatSpeed = 1.0f;      // 위아래로 움직이는 속도
    public float floatHeight = 0.1f;     // 이동 높이 (유닛)

    [Header("깜빡임 효과 (투명도 변화)")]
    public float flickerSpeed = 1.5f;    // 깜빡이는 속도
    public float alphaMin = 0.25f;       // 최소 알파값
    public float alphaMax = 0.45f;       // 최대 알파값

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    void Update()
    {
        // 부유 효과
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0f, offsetY, 0f);

        // 깜빡임 효과 (알파값만 조절)
        float alpha = Mathf.Lerp(alphaMin, alphaMax, (Mathf.Sin(Time.time * flickerSpeed) + 1f) / 2f);
        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, alpha);
    }
}
