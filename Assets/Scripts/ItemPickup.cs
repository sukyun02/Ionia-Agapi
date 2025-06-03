using UnityEngine;

public class ItemPickup2D : MonoBehaviour
{
    public AudioClip pickupSFX;
    public float volume = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 콘솔 확인용
            Debug.Log("플레이어가 아이템을 먹었다!");

            if (pickupSFX != null)
                AudioSource.PlayClipAtPoint(pickupSFX, transform.position, volume);

            Destroy(gameObject);
        }
    }
}
