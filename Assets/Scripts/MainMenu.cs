using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    void Start()
    {

    }

    public void OnPressGameStart()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnPressExit()
    {
        Application.Quit();
    }
}
