using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts; // 하트 오브젝트 배열
    private int health;

    void Start()
    {
        health = hearts.Length;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))  // 태그 비교
        {
            Debug.Log("가시에 닿음!");
            TakeDamage(1);  // 데미지 처리
        }
    }



    void TakeDamage(int amount)
    {
        if (health <= 0) return;

        health -= amount;

        if (health >= 0 && health < hearts.Length)
        {
            hearts[health].SetActive(false); // 하트 비활성화
        }

        if (health <= 0)
        {
            Debug.Log("게임 오버");
            // 게임 오버 처리
        }
    }
}
