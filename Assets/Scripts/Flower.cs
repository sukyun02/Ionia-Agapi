using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerItem : MonoBehaviour
{
    private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                case "Stage1":
                    nextSceneName = "Home1";
                    break;
                case "Home1":
                    nextSceneName = "Stage2";
                    break;
                case "Stage2":
                    nextSceneName = "Home2";
                    break;
                case "Home2":
                    nextSceneName = "Stage3";
                    break;
                case "Stage3":
                    nextSceneName = "Home3";
                    break;
                case "Home3":
                    nextSceneName = "Stage4";
                    break;

                case "Stage4":
                    nextSceneName = "Home4";
                    break;


                default:
                    Debug.LogWarning("다음 씬 설정이 없음: " + currentScene);
                    return;
            }

            Invoke("LoadNextScene", 1.5f);
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadSceneAsync(nextSceneName);
        }
    }
}
