using UnityEngine;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour
{
    private static Player instance;

    void Awake()
    {
        // 싱글턴 패턴으로 중복 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 씬에 존재하는 PlayerStartPoint 위치로 이동
        GameObject startPoint = GameObject.Find("PlayerStartPoint");
        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("PlayerStartPoint 오브젝트가 이 씬에 없습니다.");
        }
    }
}

// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class Player : MonoBehaviour
// {
//     void Awake()
//     {
//         DontDestroyOnLoad(gameObject);
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }
//
//     void OnDestroy()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }
//
//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         GameObject startPoint = GameObject.Find("PlayerStartPoint");
//         if (startPoint != null)
//         {
//             transform.position = startPoint.transform.position;
//         }
//         else
//         {
//             Debug.LogWarning("PlayerStartPoint 오브젝트를 찾을 수 없습니다.");
//         }
//     }
// }
