using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public MonoBehaviour[] scriptsToDisable;
    public AudioClip deathSound;
    private AudioSource audioSource;
    private bool isGameOver = false;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ResetState();
    }

    void ResetState()
    {
        isGameOver = false;

        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isGameOver && other.CompareTag("Spike"))
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }

        // 사운드 재생
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        StartCoroutine(RestartSceneAfterDelay(2f));
    }

    IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetState();
    }
}
