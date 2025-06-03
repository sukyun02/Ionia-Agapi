using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private static HealthUI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }
}
