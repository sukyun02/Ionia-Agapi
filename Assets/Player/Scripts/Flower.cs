using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ¥Ÿ¿Ω æ¿ ∑ŒµÂ (æ¿ ∫ÙµÂ º¯º≠ ±‚¡ÿ)
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
