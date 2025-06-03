using UnityEngine;

public class GameClearForceShow : MonoBehaviour
{
    void Start()
    {
        // 이름이 "GameClearUI"인 오브젝트를 찾아서 무조건 켬
        GameObject ui = GameObject.Find("GameClearUI");
        if (ui != null)
        {
            ui.SetActive(true);
            Debug.Log("무조건 GameClear UI 켜짐");
        }
        else
        {
            Debug.LogWarning("GameClearUI 오브젝트를 찾지 못함");
        }
    }
}
